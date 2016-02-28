package ts.ticketmanagement;

import android.content.Intent;
import android.os.AsyncTask;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;

public class LoginActivity extends ActivityBase {

    private ProgressBar progressbar;

    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        Log.d("TICKET_MANAGEMENT", "LoginActivity:onCreate");
        setContentView(R.layout.activity_login);
        progressbar = (ProgressBar)findViewById(R.id.login_prbAuthoriseProgress);

        if(tryPopulateUserCredentials("Login")){
            new Intent(this, MainActivity.class);
            Toast.makeText(getApplicationContext(), "User credentials stored on Phone already!", Toast.LENGTH_LONG).show();
        }
    }

    @Override
    protected void onResume(){
        super.onResume();
        Log.d("TICKET_MANAGEMENT", "LoginActivity:onResume");
        tryPopulateUserCredentials("Login");
    }

    public void authenticateOnClick(View pView){
        Log.d("TICKET_MANAGEMENT", "LoginActivity:authenticateOnClick");
        TextView txtUsername = (TextView)findViewById(R.id.login_lblUsername);
        username = txtUsername.getText().toString();

        if(username.isEmpty()){
            showMessageBox("Login", "Username Required", "Please enter your username.");
            return;
        }

        new API_GetUserToken().execute();
    }

    private class API_GetUserToken extends AsyncTask<Void, Void, String> {

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


            if(response == null){
                showMessageBox("Login", "Error Getting Token", "An error has occurred trying to get your" + " user token, please check the Username and try again");
                Log.e("TICKET_MANAGEMENT", "LoginActivity-API_GetUserToken:onPostExecute: Error: null from doInBackground, assumed error occurred.");
                progressbar.setVisibility(View.GONE);
            }

            try {
                JSONObject json = new JSONObject(response);
                if(json.getString("contentType").contains("UserTokenAndIsInternal")){
                    json = json.getJSONObject("data");

                    userToken = json.getString("userToken");
                    isInternal = Boolean.parseBoolean(json.getString("isInternal"));

                }
            }catch (Exception e){
                Log.e("TICKET_MANAGEMENT","TicketsActivity-API_GetUserToken:onPostExecute: JSON Exception - Message: " + e.getMessage());
            }

            if(storeCredentials(username, userToken, isInternal)){
                Intent intentWithData = new Intent();
                intentWithData.putExtra(getString(R.string.user_username), username);
                intentWithData.putExtra(getString(R.string.user_token), userToken);
                setResult(RESULT_OK, intentWithData);
            } else
                setResult(RESULT_CANCELED);

            progressbar.setVisibility(View.GONE);

            finish();
        }
    }
}