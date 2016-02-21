package ts.ticketmanagement;

import android.content.Intent;
import android.os.AsyncTask;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
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

public class TicketsActivity extends ActivityBase {

    private ProgressBar progressbar;
    private List<Ticket> tickets;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        Log.d("TICKET_MANAGEMENT", "TicketsActivity:onCreate");
        setContentView(R.layout.activity_tickets);

        if(!tryPopulateUserCredentials("Tickets")){
            Log.d("TICKET_MANAGEMENT", "TicketsActivity:onCreate: No User Credentials");
            new Intent(this, MainActivity.class);
            Toast.makeText(getApplicationContext(), "No User credentials stored on Phone", Toast.LENGTH_LONG).show();
            return;
        }

        Log.d("TICKET_MANAGEMENT", "TicketsActivity:onCreate: User Credentials present");
        progressbar = (ProgressBar)findViewById(R.id.prbTicketsActivity);
        tickets = new ArrayList<>();

        new API_GetTickets().execute();
    }

    private Ticket getTicketFromJSONObject (JSONObject json){
        Log.d("TICKET_MANAGEMENT", "TicketsActivity:getTicketFromJSONObject");
        try{
            return new Ticket(json.getString("title"),
                    json.getString("description"), json.getString("openedByName"),
                    json.getString("ticketPriorityName"), json.getString("ticketStateName"),
                    json.getString("ticketCategoryName"), json.getString("projectName"),
                    json.getString("userAssignedToName"), json.getString("teamAssignedToName"),
                    json.getString("organisationAssignedToName"), json.getString("deadline"),
                    json.getString("lastMessage"), json.getString("lastResponse"));

        }catch(Exception e){
            Log.e("TICKET_MANAGEMENT", "TicketsActivity:getTicketFromJSONObject: Exception: " + e.getMessage());
            return null;
        }
    }

    private void registerClickCallback(){
        Log.d("TICKET_MANAGEMENT", "TicketsActivity:registerClickCallback");
        ListView list = (ListView)findViewById(R.id.lstTickets);
        list.setOnItemClickListener(new AdapterView.OnItemClickListener() {

            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                Ticket clickedTicket = tickets.get(position);
                Log.d("TICKET_MANAGEMENT", "TicketsActivity:registerClickCallback: Ticket '" + clickedTicket.getTitle() + "' clicked");
                Toast.makeText(getApplicationContext(), "Clicked: " + clickedTicket.getTitle(), Toast.LENGTH_LONG).show();
            }
        });
    }

    private class TicketListAdapter extends ArrayAdapter<Ticket> {

        public TicketListAdapter() {
            super(TicketsActivity.this, R.layout.ticket_list_item, tickets);
            Log.d("TICKET_MANAGEMENT", "TicketsActivity:TicketListAdapter: Constructor Called");
        }

        @Override
        public View getView(int position, View convertView, ViewGroup parent) {
            Log.d("TICKET_MANAGEMENT", "TicketsActivity:getView");
            View itemView = convertView;
            if(itemView == null)
                itemView = getLayoutInflater().inflate(R.layout.ticket_list_item, parent, false);

            Ticket currentTicket = tickets.get(position);

            TextView title = (TextView) itemView.findViewById(R.id.lbl_ticketList_ticketTitle);
            title.setText(currentTicket.getTitle());

            TextView description = (TextView) itemView.findViewById(R.id.lbl_ticketList_ticketDescription);
            description.setText(currentTicket.getDescription());

            Log.d("TICKET_MANAGEMENT", "TicketsActivity:getView: Set ticket_list_item values");

            return itemView;
        }
    }

    private class API_GetTickets extends AsyncTask<Void, Void, String> {

        protected void onPreExecute(){
            Log.d("TICKET_MANAGEMENT", "TicketsActivity-API_GetTickets");
            progressbar.setVisibility(View.VISIBLE);
        }

        protected String doInBackground(Void... urls) {
            Log.d("TICKET_MANAGEMENT", "TicketsActivity-API_GetTickets:doInBackground");
            try{
                URL url = new URL(getString(R.string.api_url) + getString(R.string.api_tickets_getTickets1) + username + getString(R.string.api_tickets_getTickets2) + userToken);
                HttpURLConnection urlConnection = (HttpURLConnection) url.openConnection();
                Log.d("TICKET_MANAGEMENT", "TicketsActivity-API_GetTickets: Opened HTTP URL Connection to; " + url.toString());
                try{
                    BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(urlConnection.getInputStream()));
                    StringBuilder stringBuilder = new StringBuilder();
                    String line;
                    while((line = bufferedReader.readLine()) != null)
                        stringBuilder.append(line).append("\n");
                    bufferedReader.close();
                    Log.d("TICKET_MANAGEMENT", "TicketsActivity-API_GetTickets:doInBackground: Response from API=" + stringBuilder.toString());
                    return stringBuilder.toString();
                }catch (Exception e){
                    Log.e("TICKET_MANAGEMENT", "TicketsActivity-API_GetTickets:doInBackground: Error: " + e.getMessage(), e);
                    return null;
                }
                finally {
                    urlConnection.disconnect();
                }
            }catch (Exception e){
                Log.e("TICKET_MANAGEMENT", "TicketsActivity-API_GetTickets:doInBackground: Error: " + e.getMessage(), e);
                return null;
            }
        }

        protected void onPostExecute(String response){
            Log.d("TICKET_MANAGEMENT", "TicketsActivity-API_GetTickets:onPostExecute");
            if(response == null){
                showMessageBox("Main", "Cannot Get Tickets", "Unfortunately we cannot get your tickets from the server, please check your config and try again.");
                Log.e("TICKET_MANAGEMENT","TicketsActivity-API_GetTickets:onPostExecute: Null from doInBackground");
                return;
            }

            try {
                JSONObject json = new JSONObject(response);
                if(json.getString("contentType").contains("Tickets")){
                    JSONArray jsonTickets = json.getJSONArray("data");
                    for(int i = 0; i < jsonTickets.length(); i++){
                        Ticket currentTicket = getTicketFromJSONObject(jsonTickets.getJSONObject(i));
                        if(currentTicket != null)
                            tickets.add(currentTicket);
                    }
                }
            }catch (Exception e){
                Log.e("TICKET_MANAGEMENT","TicketsActivity-API_GetTickets:onPostExecute: JSON Exception - Message: " + e.getMessage());
            }

            ListView tickets = (ListView) findViewById(R.id.lstTickets);
            tickets.setAdapter(new TicketListAdapter());
            Log.e("TICKET_MANAGEMENT", "TicketsActivity-API_GetTickets:onPostExecute: Set Adapter to Tickets List");

            registerClickCallback();

            progressbar.setVisibility(View.GONE);
        }
    }
}
