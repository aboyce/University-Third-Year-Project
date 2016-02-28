package ts.ticketmanagement;

import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.os.AsyncTask;
import android.os.Bundle;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.EditText;
import android.widget.ListView;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import org.json.JSONArray;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.ArrayList;
import java.util.List;

import Entities.Ticket;
import Entities.TicketLog;
import Helpers.JSONHelper;

public class TicketActivity extends ActivityBase {

    private int ticketId;
    private Ticket ticket;
    private List<TicketLog> ticketLogs;
    private ListView ticketLogsListView;
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

        ticketLogs = new ArrayList<>();
        ticketLogsListView = (ListView)findViewById(R.id.ticket_lstTicketLogs);
        progressbar = (ProgressBar)findViewById(R.id.ticket_prbActivity);

        new API_GetTicket().execute();
        new API_GetTicketLogs().execute();
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
            new API_GetTicketLogs().execute();
        }
        return true;
    }

    public void replyOnClick(View pView){
        Log.d("TICKET_MANAGEMENT", "TicketActivity:replyOnClick");

        final EditText editText = new EditText(this);
        AlertDialog.Builder alert = new AlertDialog.Builder(this);
        alert.setTitle("Reply to Ticket");
        alert.setView(editText);

        alert.setPositiveButton("Reply", new DialogInterface.OnClickListener() {

            public void onClick(DialogInterface dialog, int which) {
                Log.d("TICKET_MANAGEMENT", "TicketActivity:replyOnClick: Reply Button Clicked");
            }
        });

        alert.setNegativeButton("Internal Reply", new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                Log.d("TICKET_MANAGEMENT", "TicketActivity:replyOnClick: Reply Internal Button Clicked");
            }
        });

        alert.setNeutralButton("Cancel", new DialogInterface.OnClickListener() {
            public void onClick(DialogInterface dialog, int which) {
                Log.d("TICKET_MANAGEMENT", "TicketActivity:replyOnClick: Cancel Button Clicked");
            }});

        alert.show();
    }


    private class TicketLogsListAdapter extends ArrayAdapter<TicketLog> {

        public TicketLogsListAdapter() {
            super(TicketActivity.this, R.layout.ticket_log_list_item_left, ticketLogs);
            Log.d("TICKET_MANAGEMENT", "TicketsActivity:TicketListAdapter: Constructor Called");
        }

        @Override
        public View getView(int position, View convertView, ViewGroup parent) {
            Log.d("TICKET_MANAGEMENT", "TicketActivity:getView");

            TicketLog currentTicketLog = ticketLogs.get(position);
            View itemView = convertView;

            try{
                if(itemView == null){
                    if(currentTicketLog.getFromInternal())
                        itemView = getLayoutInflater().inflate(R.layout.ticket_log_list_item_right, parent, false);
                    else
                        itemView = getLayoutInflater().inflate(R.layout.ticket_log_list_item_left, parent, false);
                }

                TextView submittedBy = (TextView) itemView.findViewById(R.id.ticketLogList_lblLogCreator);
                submittedBy.setText(currentTicketLog.getSubmittedBy());
                TextView message = (TextView) itemView.findViewById(R.id.ticketLogList_lblLogMessage);
                message.setText(currentTicketLog.getMessage());

                if(currentTicketLog.getIsInternal()){
                    itemView.setBackground(getDrawable(R.drawable.ticket_list_item_border_default));
                }
                else{
                    if(currentTicketLog.getFromInternal())
                        itemView.setBackground(getDrawable(R.drawable.ticket_list_item_border_success));
                    else
                        itemView.setBackground(getDrawable(R.drawable.ticket_list_item_border_primary));
                }
            } catch (Exception e){
                Log.e("TICKET_MANAGEMENT", "TicketsActivity:getView: Error when setting ticketLog_list_item values, message: " + e.getMessage());
                return itemView;
            }

            return itemView;
        }
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

    private class API_GetTicketLogs extends AsyncTask<Void, Void, String> {

        protected void onPreExecute(){
            Log.d("TICKET_MANAGEMENT", "TicketActivity-API_GetTicketLogs");
            ticketLogsListView.setVisibility(View.GONE);
            progressbar.setVisibility(View.VISIBLE);
        }

        protected String doInBackground(Void... urls) {
            Log.d("TICKET_MANAGEMENT", "TicketActivity-API_GetTicketLogs:doInBackground");
            try{
                URL url = new URL(getString(R.string.api_url)
                        + getString(R.string.api_tickets_getTicketLogs1) + ticketId
                        + getString(R.string.api_tickets_getTicketLogs2) + username
                        + getString(R.string.api_tickets_getTicketLogs3) + userToken);
                HttpURLConnection urlConnection = (HttpURLConnection) url.openConnection();
                Log.d("TICKET_MANAGEMENT", "TicketActivity-API_GetTicketLogs: Opened HTTP URL Connection to; " + url.toString());
                try{
                    BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(urlConnection.getInputStream()));
                    StringBuilder stringBuilder = new StringBuilder();
                    String line;
                    while((line = bufferedReader.readLine()) != null)
                        stringBuilder.append(line).append("\n");
                    bufferedReader.close();
                    Log.d("TICKET_MANAGEMENT", "TicketActivity-API_GetTicketLogs:doInBackground: Response from API=" + stringBuilder.toString());
                    return stringBuilder.toString();
                }catch (Exception e){
                    Log.e("TICKET_MANAGEMENT", "TicketActivity-API_GetTicketLogs:doInBackground: Error: " + e.getMessage(), e);
                    return null;
                }
                finally {
                    urlConnection.disconnect();
                }
            }catch (Exception e){
                Log.e("TICKET_MANAGEMENT", "TicketActivity-API_GetTicketLogs:doInBackground: Error: " + e.getMessage(), e);
                return null;
            }
        }

        protected void onPostExecute(String response){
            Log.d("TICKET_MANAGEMENT", "TicketActivity-API_GetTicketLogs:onPostExecute");
            if(response == null || response == "null"){
                showMessageBox("Main", "Cannot Get Ticket", "Unfortunately we cannot get your ticket from the server, please check your config and try again.");
                Log.e("TICKET_MANAGEMENT", "TicketActivity-API_GetTicketLogs:onPostExecute: Null from doInBackground");
                progressbar.setVisibility(View.GONE);
                return;
            }

            try {
                JSONObject json = new JSONObject(response);
                if(json.getString("contentType").contains("TicketLogs")){
                    JSONArray jsonTicketLogs = json.getJSONArray("data");
                    for(int i = 0; i < jsonTicketLogs.length(); i++){
                        TicketLog currentTicketLog = JSONHelper.getTicketLogFromJSONObject(jsonTicketLogs.getJSONObject(i));
                        if(currentTicketLog != null)
                            ticketLogs.add(currentTicketLog);
                    }
                }
            }catch (Exception e){
                Log.e("TICKET_MANAGEMENT","TicketsActivity-API_GetTicketLogs:onPostExecute: JSON Exception - Message: " + e.getMessage());
            }

            ListView ticketLogs = (ListView) findViewById(R.id.ticket_lstTicketLogs);
            ticketLogs.setAdapter(new TicketLogsListAdapter());
            Log.e("TICKET_MANAGEMENT", "TicketActivity-API_GetTicketLogs:onPostExecute: Set Adapter to Tickets List");

            progressbar.setVisibility(View.GONE);
            ticketLogsListView.setVisibility(View.VISIBLE);
        }
    }
}
