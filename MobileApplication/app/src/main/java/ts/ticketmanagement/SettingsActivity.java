package ts.ticketmanagement;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.AsyncTask;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.Objects;

public class SettingsActivity extends ActivityBase {

    ProgressBar progressBar;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        Log.d("TICKET_MANAGEMENT", "SettingsActivity:onCreate");
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_settings);
        progressBar = (ProgressBar) findViewById(R.id.prbSettingsActivity);

        if(!tryPopulateUserCredentials("Settings")){
            Toast.makeText(getApplicationContext(), "No User credentials stored on Phone", Toast.LENGTH_LONG).show();
        }
    }

    public void removeUserOnClick(View pView){
        Log.d("TICKET_MANAGEMENT", "SettingsActivity:removeUserOnClick");
        new API_DeactivateUserCredentials().execute();
    }

    private void removeUserFromPhone(){
        SharedPreferences sharedPreferences = this.getSharedPreferences(getString(R.string.persistent_storage_name), Context.MODE_PRIVATE);
        SharedPreferences.Editor editor = sharedPreferences.edit();
        editor.remove(getString(R.string.persistent_storage_user_username));
        editor.remove(getString(R.string.persistent_storage_user_token));

        if(!editor.commit()){
            Log.d("TICKET_MANAGEMENT", "SettingsActivity:removeUserOnClick: Failed to remove User from Shared Preferences.");
            return;
        }

        Log.d("TICKET_MANAGEMENT", "SettingsActivity:removeUserOnClick: Removed User from Shared Preferences");
        username = "";
        userToken = "";

        startActivityForResult(new Intent(this, LoginActivity.class), LOGIN_FROM_MAIN);
        Toast.makeText(getApplicationContext(), "User Token Deactivated from Server and removed from Phone!", Toast.LENGTH_LONG).show();
    }

    class API_DeactivateUserCredentials extends AsyncTask<Void, Void, String> {

        protected void onPreExecute(){
            Log.d("TICKET_MANAGEMENT", "SettingsActivity-API_DeactivateUserCredentials");
            progressBar.setVisibility(View.VISIBLE);
        }

        protected String doInBackground(Void... urls) {
            Log.d("TICKET_MANAGEMENT", "SettingsActivity-API_DeactivateUserCredentials:doInBackground");
            try{
                URL url = new URL(getString(R.string.api_url) + getString(R.string.api_user_deactivateUserToken1) + username + getString(R.string.api_user_deactivateUserToken2) + userToken);
                HttpURLConnection urlConnection = (HttpURLConnection) url.openConnection();
                Log.d("TICKET_MANAGEMENT", "SettingsActivity-API_DeactivateUserCredentials: Opened HTTP URL Connection to; " + url.toString());
                try{
                    BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(urlConnection.getInputStream()));
                    StringBuilder stringBuilder = new StringBuilder();
                    String line;
                    while((line = bufferedReader.readLine()) != null)
                        stringBuilder.append(line).append("\n");
                    bufferedReader.close();
                    Log.d("TICKET_MANAGEMENT", "SettingsActivity-API_DeactivateUserCredentials:doInBackground: Response from API=" + stringBuilder.toString());
                    return stringBuilder.toString();
                }catch (Exception e){
                    Log.e("TICKET_MANAGEMENT", "SettingsActivity-API_DeactivateUserCredentials:doInBackground: Error: " + e.getMessage(), e);
                    return null;
                }
                finally {
                    urlConnection.disconnect();
                }
            }catch (Exception e){
                Log.e("TICKET_MANAGEMENT", "SettingsActivity-API_DeactivateUserCredentials:doInBackground: Error: " + e.getMessage(), e);
                return null;
            }
        }

        protected void onPostExecute(String response){
            Log.d("TICKET_MANAGEMENT", "SettingsActivity-API_DeactivateUserCredentials:onPostExecute");
            if(response == null || response.contains("null")){
                showMessageBox("Main", "Error Deactivating Credentials", "An error has occurred trying to deactivate your user token, please check the state on the Web Application.");
                Log.e("TICKET_MANAGEMENT", "SettingsActivity-API_DeactivateUserCredentials:onPostExecute: Error: " + response);
                return;
            }

            response = response.replace("\"","");
            Log.d("TICKET_MANAGEMENT", "SettingsActivity-API_DeactivateUserCredentials:onPostExecute: Response=" + response);

            if(Objects.equals(response, "true") || response.contains("true"))
                removeUserFromPhone();
            else {
                showMessageBox("Main", "Cannot Deactivate Credentials", "Unfortunately we cannot deactivate your User Token, please check your config and try again.");
                Log.d("TICKET_MANAGEMENT", "SettingsActivity-API_DeactivateUserCredentials:onPostExecute: Cannot deactivate User Token");
            }

            progressBar.setVisibility(View.GONE);
        }
    }
}
