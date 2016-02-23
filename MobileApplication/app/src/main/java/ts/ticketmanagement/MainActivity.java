package ts.ticketmanagement;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.support.v4.app.ActivityCompat;
import android.support.v4.content.ContextCompat;
import android.os.AsyncTask;
import android.os.Bundle;
import android.telephony.SmsManager;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.Objects;

public class MainActivity extends ActivityBase {

    private final Integer PERMISSION_REQUEST_SMS = 1;

    private Activity currentActivity;
    private ProgressBar progressBar;
    private TextView textViewUsernameValue;

    private Intent ticketsIntent;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        Log.d("TICKET_MANAGEMENT", "'ACTIVITY_NAME':'METHOD_NAME':'INFORMATION'");
        Log.d("TICKET_MANAGEMENT", "MainActivity:onCreate");

        currentActivity = this;
        progressBar = (ProgressBar)findViewById(R.id.main_prbActivity);
        textViewUsernameValue = (TextView)findViewById(R.id.main_lblUsernameValue);

        new API_CheckConnection().execute();

        if(!tryPopulateUserCredentials("Main")){
            Log.d("TICKET_MANAGEMENT", "MainActivity:onCreate: User not configured with app.");
            startActivityForResult(new Intent(this, LoginActivity.class), LOGIN_FROM_MAIN);
        } else {
            Log.d("TICKET_MANAGEMENT", "MainActivity:onCreate: User is configured with app.");

            if (!username.isEmpty())
                textViewUsernameValue.setText(username);

            ticketsIntent = new Intent(this, TicketsActivity.class);
            new API_ConfirmUserCredentials().execute();
        }
    }

    @Override
    protected void onResume(){
        super.onResume();
        Log.d("TICKET_MANAGEMENT", "MainActivity:onResume");
        tryPopulateUserCredentials("Main");
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
                showMessageBox("Main", "User not logged in [RESULT_CANCELED]", "ah well...");
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
            Toast.makeText(getApplicationContext(), "SMS Message Sent!", Toast.LENGTH_LONG).show();
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
            showMessageBox("Main", "Permission Denied", "As you won't allow an SMS Message to be sent automatically, you will have to confirm the User Token manually.");
    }

    public void sendSMSOnClick(View pView){
        Log.d("TICKET_MANAGEMENT", "MainActivity:sendSMSOnClick");
        trySendSMSMessage();
    }

    public void checkConnectionOnClick(View pView){
        Log.d("TICKET_MANAGEMENT", "MainActivity:checkConnectionOnClick");
        new API_CheckConnection().execute();
    }

    public void authoriseOnClick(View pView){
        Log.d("TICKET_MANAGEMENT", "MainActivity:authoriseOnClick");
        ticketsIntent = new Intent(this, TicketsActivity.class);
        new API_ConfirmUserCredentials().execute();
    }
    private class API_CheckConnection extends AsyncTask<Void, Void, String> {

        protected void onPreExecute(){
            Log.d("TICKET_MANAGEMENT", "LoginActivity-API_CheckConnection");
            progressBar.setVisibility(View.VISIBLE);
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
                showMessageBox("Main", "Error Confirming Credentials", "An error has occurred trying to confirm the connection.");
                Log.e("TICKET_MANAGEMENT", "LoginActivity-API_CheckConnections:onPostExecute: Error: " + response);
                return;
            }

            response = response.replace("\"","");

            Log.d("TICKET_MANAGEMENT", "LoginActivity-API_CheckConnection:onPostExecute: Response=" + response);

            CheckBox checkBox = (CheckBox)findViewById(R.id.main_chkConnection);

            if(Objects.equals(response, "true") || Objects.equals(response, "true\n") || response.contains("true"))
                checkBox.setChecked(true);
            else {
                checkBox.setChecked(false);
                showMessageBox("Main", "Cannot Confirm Connection", "Unfortunately we cannot confirm your connection, please check your config and try again.");
                Log.d("TICKET_MANAGEMENT", "LoginActivity-API_CheckConnection:onPostExecute: Cannot confirm connection.");
            }

            progressBar.setVisibility(View.GONE);
        }
    }

    private class API_ConfirmUserCredentials extends AsyncTask<Void, Void, String> {

        protected void onPreExecute(){
            Log.d("TICKET_MANAGEMENT", "LoginActivity-API_ConfirmUserCredentials");
            progressBar.setVisibility(View.VISIBLE);
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
                showMessageBox("Main", "Error Confirming Credentials", "An error has occurred trying to confirm your user token, please check the configuration and try again");
                Log.e("TICKET_MANAGEMENT","LoginActivity-API_ConfirmUserCredentials:onPostExecute: Error: " + response);
                return;
            }

            response = response.replace("\"","");
            Log.d("TICKET_MANAGEMENT", "LoginActivity-API_ConfirmUserCredentials:onPostExecute: Response=" + response);

            CheckBox authorisedCheckBox = (CheckBox)findViewById(R.id.main_chkAccountAuthorised);
            Button sendSMSButton = (Button)findViewById(R.id.main_btnSendSMS);

            if(Objects.equals(response, "true") || response.contains("true")){
                authorisedCheckBox.setChecked(true);
                sendSMSButton.setVisibility(View.GONE);
                startActivity(ticketsIntent);
            }
            else {
                authorisedCheckBox.setChecked(false);
                sendSMSButton.setVisibility(View.VISIBLE);
                showMessageBox("Main", "Cannot Confirm Credentials", "Unfortunately we cannot confirm your credentials, please check your config and try again.");
                Log.d("TICKET_MANAGEMENT", "LoginActivity-API_ConfirmUserCredentials:onPostExecute: Cannot confirm User credentials");
            }

            progressBar.setVisibility(View.GONE);
        }
    }
}