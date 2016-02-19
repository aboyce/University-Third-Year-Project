package ts.ticketmanagement;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.pm.PackageManager;
import android.support.v4.app.ActivityCompat;
import android.support.v4.content.ContextCompat;
import android.support.v7.app.AppCompatActivity;
import android.os.AsyncTask;
import android.os.Bundle;
import android.telephony.SmsManager;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.CheckBox;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.Objects;

public class MainActivity extends AppCompatActivity {

    private final Integer LOGIN_FROM_MAIN = 0;
    private final Integer PERMISSION_REQUEST_SMS = 1;

    private Activity currentActivity;
    private ProgressBar progressbar;
    private TextView textViewUsernameValue;

    private String username;
    private String userToken;

    private Intent ticketsIntent;
    private Intent loginIntent;

    @Override
    public boolean onCreateOptionsMenu(Menu menu){
        getMenuInflater().inflate(R.menu.ts_menu, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item){
        if(item.getItemId() == R.id.ts_menu_settings){
            Toast.makeText(getApplicationContext(), "Selected dem settings", Toast.LENGTH_LONG).show();
        }
        return true;
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        Log.d("TICKET_MANAGEMENT", "'ACTIVITY_NAME':'METHOD_NAME':'INFORMATION'");
        Log.d("TICKET_MANAGEMENT", "MainActivity:onCreate");

        setContentView(R.layout.activity_main);

        currentActivity = this;
        progressbar = (ProgressBar)findViewById(R.id.prbMainActivity);
        textViewUsernameValue = (TextView)findViewById(R.id.lblUsernameValue);

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

            if (!username.isEmpty())
                textViewUsernameValue.setText(username);

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

        textViewUsernameValue.setText(username);

        new AlertDialog.Builder(this)
                .setTitle("Credentials have been Saved!")
                .setMessage("Your User Token will have to be confirmed before use.")
                .setPositiveButton("Send Text Now", new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialogInterface, int which) {
                        Log.d("TICKET_MANAGEMENT", "MainActivity:handleLoginFromMain: SendTextNow Clicked.");
                        trySendSMSMessage();
                    }
                })
                .setNegativeButton("Wait for Web Application", new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialogInterface, int which) {
                        Log.d("TICKET_MANAGEMENT", "MainActivity:handleLoginFromMain: WaitForWebApplication Clicked.");
                    }
                })
                .show();
    }

    private Boolean trySendSMSMessage()
    {
        if (ContextCompat.checkSelfPermission(currentActivity, android.Manifest.permission.SEND_SMS) != PackageManager.PERMISSION_GRANTED) {
            Log.d("TICKET_MANAGEMENT", "MainActivity:trySendSMSMessage: No permission to send SMS");
            ActivityCompat.requestPermissions(currentActivity, new String[]{android.Manifest.permission.SEND_SMS}, PERMISSION_REQUEST_SMS);
            return false;
        } else {
            Log.d("TICKET_MANAGEMENT", "MainActivity:trySendSMSMessage: Permission to send SMS");
            String number = getString(R.string.api_phone_numberToConfirm);
            String message = getString(R.string.api_phone_body) + userToken;
            SmsManager.getDefault().sendTextMessage(number, null, message, null, null);
            Log.d("TICKET_MANAGEMENT", "MainActivity:trySendSMSMessage: Text message sent.");
            return true;
        }
    }

    @Override
    public void onRequestPermissionsResult(int requestCode, String permissions[], int[] grantResults ){
        if(requestCode != PERMISSION_REQUEST_SMS) return;

        if(grantResults.length > 0 && grantResults[0] == PackageManager.PERMISSION_GRANTED)
            trySendSMSMessage();
        else
            showMessageBox("Permission Denied", "As you won't allow an SMS Message to be sent automatically, you will have to confirm the User Token manually.");
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

    public void sendSMSOnClick(View pView){
        Log.d("TICKET_MANAGEMENT", "MainActivity:sendSMSOnClick");
        trySendSMSMessage();
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
        textViewUsernameValue.setText(getString(R.string.main_lbl_usernameValue));

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

            if(Objects.equals(response, "true") || Objects.equals(response, "true\n") || response.contains("true"))
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
                Log.e("TICKET_MANAGEMENT", "LoginActivity-API_ConfirmUserCredentials:doInBackground: Error: " + e.getMessage(), e);
                return null;
            }
        }

        protected void onPostExecute(String response){
            Log.d("TICKET_MANAGEMENT", "LoginActivity-API_ConfirmUserCredentials:onPostExecute");
            if(response == null || response.contains("null")){
                showMessageBox("Error Confirming Credentials", "An error has occurred trying to confirm your user token, please check the configuration and try again");
                Log.e("TICKET_MANAGEMENT","LoginActivity-API_ConfirmUserCredentials:onPostExecute: Error: " + response);
                return;
            }

            response = response.replace("\"","");
            Log.d("TICKET_MANAGEMENT", "LoginActivity-API_ConfirmUserCredentials:onPostExecute: Response=" + response);

            if(Objects.equals(response, "true") || response.contains("true"))
                startActivity(ticketsIntent);
            else {
                showMessageBox("Cannot Confirm Credentials", "Unfortunately we cannot confirm your credentials, please check your config and try again.");
                Log.d("TICKET_MANAGEMENT", "LoginActivity-API_ConfirmUserCredentials:onPostExecute: Cannot confirm User credentials");
            }

            progressbar.setVisibility(View.GONE);
        }
    }
}

