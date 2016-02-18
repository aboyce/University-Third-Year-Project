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
import android.widget.CheckBox;
import android.widget.ProgressBar;
import android.widget.TextView;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;

public class MainActivity extends AppCompatActivity {

    private final Integer LOGIN_FROM_MAIN = 0;

    ProgressBar progressbar;

    private String username;
    private String userToken;

    private Intent ticketsIntent;
    private Intent loginIntent;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        Log.d("TICKET_MANAGEMENT", "'ACTIVITY_NAME':'METHOD_NAME':'INFORMATION'");
        Log.d("TICKET_MANAGEMENT", "MainActivity:onCreate");

        setContentView(R.layout.activity_main);

        progressbar = (ProgressBar)findViewById(R.id.prbMainActivity);

        // Will let the user know that the application is connected or not.
        new API_CheckConnection().execute();

        // If the phone cannot find a valid username/userToken, then assume they are
        // not 'logged in', so send them to the Login Page.
        if(!userConfiguredWithApplication()){
            Log.d("TICKET_MANAGEMENT", "MainActivity:onCreate: User not configured with app.");
            loginIntent = new Intent(this, LoginActivity.class);
            startActivityForResult(loginIntent, LOGIN_FROM_MAIN);
        } else {
            Log.d("TICKET_MANAGEMENT", "MainActivity:onCreate: User is configured with app.");

            TextView txtUsername = (TextView)findViewById(R.id.lblUsernameValue);
            if (!username.isEmpty())
                txtUsername.setText(username.toString());

            ticketsIntent = new Intent(this, TicketsActivity.class);
            new API_ConfirmUserCredentials().execute();
        }
    }

    @Override
    protected void onActivityResult(int pRequestCode, int pResultCode, Intent pData){
        Log.d("TICKET_MANAGEMENT", "MainActivity:onActivityResult");
        if(pRequestCode == LOGIN_FROM_MAIN){
            Log.d("TICKET_MANAGEMENT", "MainActivity:onActivityResult:LoginFromMain");
            if (pResultCode == RESULT_OK){
                Log.d("TICKET_MANAGEMENT", "MainActivity:onActivityResult:LoginFromMain Result: OK ");
                handleLoginFromMain(pData);
            }
            else if(pResultCode == RESULT_CANCELED){
                Log.d("TICKET_MANAGEMENT", "MainActivity:onActivityResult:LoginFromMain Result: Canceled ");
                // TODO: remove this eventually
                showMessageBox("User not logged in [RESULT_CANCELED]", "ah well...");

                // TODO: maybe a sensible popup box
            }
        }
    }

    private void handleLoginFromMain(Intent pData)
    {
        if(pData.hasExtra(getString(R.string.user_username)))
            username = pData.getStringExtra(getString(R.string.user_username));
        if(pData.hasExtra(getString(R.string.user_token)))
            userToken = pData.getStringExtra(getString(R.string.user_token));

        Log.d("TICKET_MANAGEMENT", "MainActivity:handleLoginFromMain: User information is now stored within the app.");

        AlertDialog.Builder messageBox = new AlertDialog.Builder(this);
        messageBox.setTitle("Credentials have been Saved!");
        messageBox.setMessage("Your credentials have been saved locally. The User Token will have to be confirmed before use, you can do that now via text or via the web application.");
        messageBox.setItems(new CharSequence[]{"Send Text Now", "Wait for Web Application"},
                new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialogInterface, int option){
                        if(option == 0){
                            //Intent intent = new Intent(Intent.ACTION_SENDTO, Uri.parse("smsto:555555"));
                            //intent.putExtra("sms_body", "The message body");
                            //startActivity(intent);
                            // TODO: Get back from sms....
                        }
                        else if (option == 1){
                        }
                    }});
        messageBox.create().show();
    }

    private boolean userConfiguredWithApplication()    {
        Log.d("TICKET_MANAGEMENT","MainActivity:userConfiguredWithApplication");
        SharedPreferences sharedPreferences = this.getSharedPreferences(getString(R.string.persistent_storage_name), Context.MODE_PRIVATE);

        if(sharedPreferences.contains(getString(R.string.persistent_storage_user_username))) {
            username = sharedPreferences.getString(getString(R.string.persistent_storage_user_username), null);
            Log.d("TICKET_MANAGEMENT", "MainActivity:userConfiguredWithApplication: Contained username.");
        } else return false;

        if(sharedPreferences.contains(getString(R.string.persistent_storage_user_token))){
            userToken = sharedPreferences.getString(getString(R.string.persistent_storage_user_token), null);
            Log.d("TICKET_MANAGEMENT", "MainActivity:userConfiguredWithApplication: Contained userToken.");
        } else return false;

        return true;
    }

    public void checkConnectionOnClick(View pView){
        Log.d("TICKET_MANAGEMENT", "MainActivity:checkConnectionOnClick");
        new API_CheckConnection().execute();
    }

    public void reEnterCredentialsOnClick(View pView) {
        Log.d("TICKET_MANAGEMENT", "MainActivity:reEnterCredentialsOnClick");
        loginIntent = new Intent(this, LoginActivity.class);
        startActivityForResult(loginIntent, LOGIN_FROM_MAIN);
    }

    public void authoriseOnClick(View pView){
        Log.d("TICKET_MANAGEMENT", "MainActivity:authoriseOnClick");
        ticketsIntent = new Intent(this, TicketsActivity.class);
        new API_ConfirmUserCredentials().execute();
    }

    public void removeUserOnClick(View pView){
        Log.d("TICKET_MANAGEMENT", "MainActivity:removeUserOnClick");
        SharedPreferences sharedPreferences = this.getSharedPreferences(getString(R.string.persistent_storage_name), Context.MODE_PRIVATE);
        SharedPreferences.Editor editor = sharedPreferences.edit();
        editor.remove(getString(R.string.persistent_storage_user_username));
        editor.remove(getString(R.string.persistent_storage_user_token));

        if(!editor.commit()){
            Log.d("TICKET_MANAGEMENT", "MainActivity:removeUserOnClick: Failed to remove User from Shared Preferences.");
            return;
        }

        Log.d("TICKET_MANAGEMENT", "MainActivity:removeUserOnClick: Removed User from Shared Preferences");
        username = "";
        userToken = "";

        TextView txtUsername = (TextView)findViewById(R.id.lblUsernameValue);
        txtUsername.setText(getString(R.string.main_lbl_usernameValue));

        showMessageBox("Removed User", "Your User information has been removed from this device. For safety, deactivate your User Token.");

        loginIntent = new Intent(this, LoginActivity.class);
        startActivityForResult(loginIntent, LOGIN_FROM_MAIN);
    }

    private void showMessageBox(String title, String message){
        Log.d("TICKET_MANAGEMENT","MainActivity:showMessageBox: Title='" + title + "' Message= " + message);
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

    class API_CheckConnection extends AsyncTask<Void, Void, String> {

        protected void onPreExecute(){
            Log.d("TICKET_MANAGEMENT", "LoginActivity-API_CheckConnection");
            progressbar.setVisibility(View.VISIBLE);
        }

        protected String doInBackground(Void... urls) {
            Log.d("TICKET_MANAGEMENT", "LoginActivity-API_CheckConnection:doInBackground");
            try{
                URL url = new URL(getString(R.string.api_url) + getString(R.string.api_confirmConnection));
                HttpURLConnection urlConnection = (HttpURLConnection) url.openConnection();
                Log.d("TICKET_MANAGEMENT", "LoginActivity-API_CheckConnection: Opened HTTP URL Connection to; " + url.toString());
                try{
                    BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(urlConnection.getInputStream()));
                    StringBuilder stringBuilder = new StringBuilder();
                    String line;
                    while((line = bufferedReader.readLine()) != null)
                        stringBuilder.append(line).append("\n");
                    bufferedReader.close();
                    Log.d("TICKET_MANAGEMENT", "LoginActivity-API_CheckConnection:doInBackground: Response from API=" + stringBuilder.toString());
                    return stringBuilder.toString();
                }catch (Exception e){
                    Log.e("TICKET_MANAGEMENT", "LoginActivity-API_CheckConnection:doInBackground: Error: " + e.getMessage(), e);
                    return null;
                }
                finally {
                    urlConnection.disconnect();
                }
            }catch (Exception e){
                Log.e("TICKET_MANAGEMENT", "LoginActivity-API_CheckConnection:doInBackground: Error: " + e.getMessage(), e);
                return null;
            }
        }

        protected void onPostExecute(String response){
            Log.d("TICKET_MANAGEMENT", "LoginActivity-API_CheckConnection:onPostExecute");
            if(response == null){
                showMessageBox("Error Confirming Credentials", "An error has occurred trying to confirm the connection.");
                Log.e("TICKET_MANAGEMENT", "LoginActivity-API_CheckConnections:onPostExecute: Error: " + response);
                return;
            }

            response = response.replace("\"","");

            Log.d("TICKET_MANAGEMENT", "LoginActivity-API_CheckConnection:onPostExecute: Response=" + response);

            CheckBox checkBox = (CheckBox)findViewById(R.id.chkConnection);

            if(response == "true" || response == "true\n" || response.contains("true"))
                checkBox.setChecked(true);
            else {
                checkBox.setChecked(false);
                showMessageBox("Cannot Confirm Connection", "Unfortunately we cannot confirm your connection, please check your config and try again.");
                Log.d("TICKET_MANAGEMENT", "LoginActivity-API_CheckConnection:onPostExecute: Cannot confirm connection.");
            }

            progressbar.setVisibility(View.GONE);
        }
    }

    class API_ConfirmUserCredentials extends AsyncTask<Void, Void, String> {

        protected void onPreExecute(){
            Log.d("TICKET_MANAGEMENT", "LoginActivity-API_ConfirmUserCredentials");
            progressbar.setVisibility(View.VISIBLE);
        }

        protected String doInBackground(Void... urls) {
            Log.d("TICKET_MANAGEMENT", "LoginActivity-API_ConfirmUserCredentials:doInBackground");
            try{
                URL url = new URL(getString(R.string.api_url) + getString(R.string.api_user_confirmUserToken1) + username + getString(R.string.api_user_confirmUserToken2) + userToken);
                HttpURLConnection urlConnection = (HttpURLConnection) url.openConnection();
                Log.d("TICKET_MANAGEMENT", "LoginActivity-API_ConfirmUserCredentials: Opened HTTP URL Connection to; " + url.toString());
                try{
                    BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(urlConnection.getInputStream()));
                    StringBuilder stringBuilder = new StringBuilder();
                    String line;
                    while((line = bufferedReader.readLine()) != null)
                        stringBuilder.append(line).append("\n");
                    bufferedReader.close();
                    Log.d("TICKET_MANAGEMENT", "LoginActivity-API_ConfirmUserCredentials:doInBackground: Response from API=" + stringBuilder.toString());
                    return stringBuilder.toString();
                }catch (Exception e){
                    Log.e("TICKET_MANAGEMENT", "LoginActivity-API_ConfirmUserCredentials:doInBackground: Error: " + e.getMessage(), e);
                    return null;
                }
                finally {
                    urlConnection.disconnect();
                }
            }catch (Exception e){
                Log.e("TICKET_MANAGEMENT", "LoginActivity-API_ConfirmUserCredentialsn:doInBackground: Error: " + e.getMessage(), e);
                return null;
            }
        }

        protected void onPostExecute(String response){
            Log.d("TICKET_MANAGEMENT", "LoginActivity-API_ConfirmUserCredentials:onPostExecute");
            if(response == null){
                showMessageBox("Error Confirming Credentials", "An error has occurred trying to confirm your user token, please check the configuration and try again");
                Log.e("TICKET_MANAGEMENT","LoginActivity-API_ConfirmUserCredentials:onPostExecute: Error: " + response);
                return;
            }

            response = response.replace("\"","");
            Log.d("TICKET_MANAGEMENT", "LoginActivity-API_ConfirmUserCredentials:onPostExecute: Response=" + response);

            if(response == "true" || response.contains("true"))
                startActivity(ticketsIntent);
            else {
                showMessageBox("Cannot Confirm Credentials", "Unfortunately we cannot confirm your credentials, please check your config and try again.");
                Log.d("TICKET_MANAGEMENT", "LoginActivity-API_ConfirmUserCredentials:onPostExecute: Cannot confirm User credentials");
            }

            progressbar.setVisibility(View.GONE);
        }
    }
}

