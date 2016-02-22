package ts.ticketmanagement;

import android.content.Intent;
import android.os.AsyncTask;
import android.os.Bundle;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;

import Entities.Ticket;
import Helpers.JSONHelper;

public class TicketActivity extends ActivityBase {

    private int ticketId;
    private Ticket ticket;

    private ProgressBar progressbar;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_ticket);

        Bundle extras = getIntent().getExtras();
        if(extras != null)
            ticketId = extras.getInt(getString(R.string.ticket_id));

        if(!tryPopulateUserCredentials("Ticket")){
            Log.d("TICKET_MANAGEMENT", "TicketActivity:onCreate: No User Credentials");
            new Intent(this, MainActivity.class);
            Toast.makeText(getApplicationContext(), "No User credentials stored on Phone", Toast.LENGTH_LONG).show();
            return;
        }

        progressbar = (ProgressBar)findViewById(R.id.ticket_prbActivity);

        new API_GetTicket().execute();
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu){
        getMenuInflater().inflate(R.menu.ts_menu, menu);
        menu.getItem(0).setVisible(true);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item){
        if(item.getItemId() == R.id.ts_menu_settings){
            startActivity(new Intent(this, SettingsActivity.class));
        }
        if(item.getItemId() == R.id.ts_menu_refresh){
            new API_GetTicket().execute();
        }
        return true;
    }


    private class API_GetTicket extends AsyncTask<Void, Void, String> {

        protected void onPreExecute(){
            Log.d("TICKET_MANAGEMENT", "TicketActivity-API_GetTicket");
            progressbar.setVisibility(View.VISIBLE);
        }

        protected String doInBackground(Void... urls) {
            Log.d("TICKET_MANAGEMENT", "TicketActivity-API_GetTicket:doInBackground");
            try{
                URL url = new URL(getString(R.string.api_url)
                        + getString(R.string.api_tickets_getTicket1) + ticketId
                        + getString(R.string.api_tickets_getTicket2) + username
                        + getString(R.string.api_tickets_getTicket3) + userToken);
                HttpURLConnection urlConnection = (HttpURLConnection) url.openConnection();
                Log.d("TICKET_MANAGEMENT", "TicketActivity-API_GetTicket: Opened HTTP URL Connection to; " + url.toString());
                try{
                    BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(urlConnection.getInputStream()));
                    StringBuilder stringBuilder = new StringBuilder();
                    String line;
                    while((line = bufferedReader.readLine()) != null)
                        stringBuilder.append(line).append("\n");
                    bufferedReader.close();
                    Log.d("TICKET_MANAGEMENT", "TicketActivity-API_GetTicket:doInBackground: Response from API=" + stringBuilder.toString());
                    return stringBuilder.toString();
                }catch (Exception e){
                    Log.e("TICKET_MANAGEMENT", "TicketActivity-API_GetTicket:doInBackground: Error: " + e.getMessage(), e);
                    return null;
                }
                finally {
                    urlConnection.disconnect();
                }
            }catch (Exception e){
                Log.e("TICKET_MANAGEMENT", "TicketActivity-API_GetTicket:doInBackground: Error: " + e.getMessage(), e);
                return null;
            }
        }

        protected void onPostExecute(String response){
            Log.d("TICKET_MANAGEMENT", "TicketActivity-API_GetTicket:onPostExecute");
            if(response == null || response == "null"){
                showMessageBox("Main", "Cannot Get Ticket", "Unfortunately we cannot get your ticket from the server, please check your config and try again.");
                Log.e("TICKET_MANAGEMENT", "TicketActivity-API_GetTicket:onPostExecute: Null from doInBackground");
                progressbar.setVisibility(View.GONE);
                return;
            }

            try {
                JSONObject json = new JSONObject(response);
                if(json.getString("contentType").contains("Ticket")){
                    ticket = JSONHelper.getTicketFromJSONObject(json.getJSONObject("data"));
                }
            }catch (Exception e){
                Log.e("TICKET_MANAGEMENT","TicketsActivity-API_GetTickets:onPostExecute: JSON Exception - Message: " + e.getMessage());
            }

            if (ticket == null){
                Log.d("TICKET_MANAGEMENT","TicketsActivity-API_GetTickets:onPostExecute: Ticket is null (logic error). ");
                progressbar.setVisibility(View.GONE);
                return;
            }


            TextView lblTicketTitle = (TextView)findViewById(R.id.ticket_lblTicketTitle);
            lblTicketTitle.setText(ticket.getTitle());

            TextView lblTicketDescription = (TextView)findViewById(R.id.ticket_lblTicketDescription);
            lblTicketDescription.setText(ticket.getDescription());

            Log.e("TICKET_MANAGEMENT", "TicketsActivity-API_GetTickets:onPostExecute: Set Adapter to Tickets List");

            progressbar.setVisibility(View.GONE);
        }
    }
}
