package ts.ticketmanagement;

import android.content.Intent;
import android.os.AsyncTask;
import android.os.Bundle;
import android.util.Log;
import android.widget.ProgressBar;
import android.widget.Toast;

import org.json.JSONArray;
import org.json.JSONObject;
import org.json.JSONTokener;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;


public class TicketsActivity extends ActivityBase {

    private ProgressBar progressbar;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        Log.d("TICKET_MANAGEMENT", "TicketsActivity:onCreate");
        setContentView(R.layout.activity_tickets);

        if(!tryPopulateUserCredentials("Tickets")){
            new Intent(this, MainActivity.class);
            Toast.makeText(getApplicationContext(), "No User credentials stored on Phone", Toast.LENGTH_LONG).show();
            return;
        }

        new API_GetTickets().execute();
    }

    class API_GetTickets extends AsyncTask<Void, Void, String> {

        protected void onPreExecute(){
            Log.d("TICKET_MANAGEMENT", "TicketsActivity-API_GetTickets");
            //progressBar.setVisibility(View.VISIBLE);
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

                String temp = json.getString("contentType");

                if(json.getString("contentType").contains("Tickets")){
                    JSONArray tickets = json.getJSONArray("data");

                    for(int i = 0; i < tickets.length(); i++){
                        JSONObject ticket =  tickets.getJSONObject(i);
                        String title = ticket.getString("title");

                    }

                }


            }catch (Exception e){
                String exception = e.getMessage();
            }

            //progressBar.setVisibility(View.GONE);
        }
    }
}
