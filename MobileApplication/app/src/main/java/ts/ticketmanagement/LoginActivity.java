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

public class LoginActivity extends AppCompatActivity {

    ProgressBar progressbar;
    String username;
    String userToken;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);

        progressbar = (ProgressBar)findViewById(R.id.prbAuthoriseProgress);
    }

    public void authenticateOnClick(View pView){
        TextView txtUsername = (TextView)findViewById(R.id.txtUsername);
         username = txtUsername.getText().toString();

        if(username.isEmpty()){
            showMessageBox("Username Required", "Please enter your username.");
            return;
        }

        new GetUserToken().execute();
    }

    private boolean storeCredentials(String username, String userToken) {
        try{ //http://developer.android.com/training/basics/data-storage/shared-preferences.html
            SharedPreferences preferences = getPreferences(Context.MODE_PRIVATE);
            SharedPreferences.Editor editor = preferences.edit();
            editor.putString(getString(R.string.user_username), username);
            editor.putString(getString(R.string.user_token), userToken);
            editor.apply();
            return true;
        } catch (Exception e){
            Log.e("ERROR", e.getMessage(), e);
        }
        return false;
    }

    private void showMessageBox(String title, String message){
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

    class GetUserToken extends AsyncTask<Void, Void, String> {

        protected void onPreExecute(){
            progressbar.setVisibility(View.VISIBLE);
            Log.d("DEBUG","API call GetUserToken");
        }

        protected String doInBackground(Void... params) {

            try{
                URL url = new URL(getString(R.string.api_url));
                HttpURLConnection urlConnection = (HttpURLConnection) url.openConnection();
                try{
                    BufferedReader bufferedReader =
                            new BufferedReader(
                                    new InputStreamReader(urlConnection.getInputStream()));
                    StringBuilder stringBuilder = new StringBuilder();
                    String line;
                    while((line = bufferedReader.readLine()) != null)
                        stringBuilder.append(line).append("\n");
                    bufferedReader.close();
                    return stringBuilder.toString();
                }catch(Exception e){
                    Log.e("ERROR", e.getMessage(), e);
                }finally {
                    urlConnection.disconnect();
                }
            }catch (Exception e){
                Log.e("ERROR", e.getMessage(), e);
            }
            return null; // Something must of gone wrong.
        }

        protected void onPostExecute(String response){

            if(response == null || userToken.isEmpty()){
                showMessageBox("Error Getting Token", "An error has occurred trying to get your" +
                        " user token, please check the configuration and try again");
                Log.e("ERROR", response);
                return;
            }

            progressbar.setVisibility(View.GONE);

            if(storeCredentials(username, userToken)){
                Intent intentWithData = new Intent();
                intentWithData.putExtra(getString(R.string.user_username), username);
                setResult(RESULT_OK, intentWithData);
            } else
                setResult(RESULT_CANCELED);
        }
    }
}

