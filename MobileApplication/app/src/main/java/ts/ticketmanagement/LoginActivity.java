package ts.ticketmanagement;

import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.AsyncTask;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.ProgressBar;
import android.widget.TextView;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.Objects;


public class LoginActivity extends AppCompatActivity {

    ProgressBar progressbar;
    String username = "";
    String userToken = "";

    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        Log.d("TICKET_MANAGEMENT", "LoginActivity:onCreate");
        setContentView(R.layout.activity_login);
        progressbar = (ProgressBar)findViewById(R.id.prbAuthoriseProgress);
    }

    public void authenticateOnClick(View pView){
        Log.d("TICKET_MANAGEMENT", "LoginActivity:authenticateOnClick");
        TextView txtUsername = (TextView)findViewById(R.id.lblUsername);
        username = txtUsername.getText().toString();

        if(username.isEmpty()){
            showMessageBox("Username Required", "Please enter your username.");
            return;
        }

        new API_GetUserToken().execute();
    }

    private boolean storeCredentials(String username, String userToken) {
        Log.d("TICKET_MANAGEMENT", "LoginActivity:storeCredentials: Username= " + username + " UserToken= " + userToken);
        try{
            SharedPreferences sharedPreferences = this.getSharedPreferences(getString(R.string.persistent_storage_name), Context.MODE_PRIVATE);
            SharedPreferences.Editor editor = sharedPreferences.edit();
            editor.putString(getString(R.string.persistent_storage_user_username), username);
            editor.putString(getString(R.string.persistent_storage_user_token), userToken);
            return editor.commit();
        } catch (Exception e){
            Log.e("TICKET_MANAGEMENT","LoginActivity:storeCredentials: Error: " + e.getMessage(), e);
        }
        return false;
    }

    private void showMessageBox(String title, String message){
        Log.d("TICKET_MANAGEMENT","TicketsActivity:showMessageBox: Title='" + title + "' Message= " + message);
        AlertDialog.Builder messageBox = new AlertDialog.Builder(this);
        messageBox.setTitle(title);
        messageBox.setMessage(message);
        messageBox.setPositiveButton("OK", new DialogInterface.OnClickListener() {
            public void onClick(DialogInterface dialogInterface, int which) {
            }
        });
        messageBox.setCancelable(true);
        messageBox.create().show();
    }

    class API_GetUserToken extends AsyncTask<Void, Void, String> {

        protected void onPreExecute(){
            Log.d("TICKET_MANAGEMENT", "LoginActivity-API_GetUserToken:onPreExecute");
            progressbar.setVisibility(View.VISIBLE);
        }

        protected String doInBackground(Void... urls) {
            Log.d("TICKET_MANAGEMENT", "LoginActivity-API_GetUserToken:doInBackground");
            try{
                URL url = new URL(getString(R.string.api_url) + getString(R.string.api_user_getNewUserToken) + username);
                HttpURLConnection urlConnection = (HttpURLConnection) url.openConnection();
                Log.d("TICKET_MANAGEMENT", "LoginActivity-API_GetUserToken:doInBackground: Opened HTTP URL Connection to; " + url.toString());
                try{
                    BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(urlConnection.getInputStream()));
                    StringBuilder stringBuilder = new StringBuilder();
                    String line;
                    while((line = bufferedReader.readLine()) != null)
                        stringBuilder.append(line).append("\n");
                    bufferedReader.close();
                    Log.d("TICKET_MANAGEMENT", "LoginActivity-API_GetUserToken:doInBackground: Response from API=" + stringBuilder.toString());
                    return stringBuilder.toString();
                }catch (Exception e){
                    Log.e("TICKET_MANAGEMENT", "LoginActivity-API_GetUserToken:doInBackground: Error: " + e.getMessage(), e);
                    return null;
                }
                finally {
                    urlConnection.disconnect();
                }
            }catch (Exception e){
                Log.e("TICKET_MANAGEMENT", "LoginActivity-API_GetUserToken:doInBackground: Error: " + e.getMessage(), e);
                return null;
            }
        }

        protected void onPostExecute(String response){
            Log.d("TICKET_MANAGEMENT", "LoginActivity-API_GetUserToken:onPostExecute");
            if(response == null || username == null || Objects.equals(username, "")){
                showMessageBox("Error Getting Token", "An error has occurred trying to get your" +
                        " user token, please check the configuration and try again");
                Log.e("TICKET_MANAGEMENT","LoginActivity-API_GetUserToken:onPostExecute: Error: " + response);
                return;
            }

            userToken = response.replace("\"","");
            progressbar.setVisibility(View.GONE);

            if(storeCredentials(username, userToken)){
                Intent intentWithData = new Intent();
                intentWithData.putExtra(getString(R.string.user_username), username);
                intentWithData.putExtra(getString(R.string.user_token), userToken);
                setResult(RESULT_OK, intentWithData);
            } else
                setResult(RESULT_CANCELED);

            finish();
        }
    }
}