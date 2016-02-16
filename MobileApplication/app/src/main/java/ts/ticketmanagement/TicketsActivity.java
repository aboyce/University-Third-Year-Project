package ts.ticketmanagement;

import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.net.Uri;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;

public class TicketsActivity extends AppCompatActivity {

    static final int LOGIN_FROM_TICKETS = 0;

    private String username;
    private String userToken;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        Log.d("TICKET_MANAGEMENT", "'ACTIVITY_NAME':'METHOD_NAME':'INFORMATION'");
        Log.d("TICKET_MANAGEMENT", "TicketsActivity:onCreate");

        // Load up the Tickets Page
        setContentView(R.layout.activity_tickets);

        // If the phone cannot find a valid username/userToken, then assume they are
        // not 'logged in', so send them to the Login Page.
        if(!userConfiguredWithApplication()){
            Log.d("TICKET_MANAGEMENT", "TicketsActivity:onCreate: User not configured with app.");
            Intent loginIntent = new Intent(this, LoginActivity.class);
            startActivityForResult(loginIntent, LOGIN_FROM_TICKETS);
        }

        // TODO: if they are do have credentials, then we need to confirm that the access token we have is usable, aka a test api call.

        // TODO: if credentials present and authenticated, then we can pull the tickets in.

        // TODO: if the credentials are not authenticated, then we could try the login again with a message box to update.
    }

    @Override
    protected void onActivityResult(int pRequestCode, int pResultCode, Intent pData){
        Log.d("TICKET_MANAGEMENT","TicketsActivity:onActivityResult");
        if(pRequestCode == LOGIN_FROM_TICKETS){
            if (pResultCode == RESULT_OK){
                if(pData.hasExtra(getString(R.string.user_username)))
                    username = pData.getStringExtra(getString(R.string.user_username));
                if(pData.hasExtra(getString(R.string.user_token)))
                    userToken = pData.getStringExtra(getString(R.string.user_token));

                // TODO: remove this eventually
                showMessageBox("User is logged in! [RESULT_OK]", "Whooo, the saved information is, Username: " + username + " User Token: " + userToken);

                // TODO: Pop up asking if it is ok to confirm the authentication via SMS
//                Intent intent = new Intent(Intent.ACTION_SENDTO, Uri.parse("smsto:555555"));
//                intent.putExtra("sms_body", "The message body");
//                startActivity(intent);

                // TODO: Get back from sms....

                // TODO: Load the tickets via an api call...

                // TODO: This may be better as a MainActivity, and then load the tickets activity??


            }
            else if(pResultCode == RESULT_CANCELED){

                // TODO: remove this eventually
                showMessageBox("User not logged in [RESULT_CANCELED]", "ah well...");

                // TODO: maybe a sensible popup box
            }
        }
    }

    private boolean userConfiguredWithApplication()    {
        Log.d("TICKET_MANAGEMENT","TicketsActivity:userConfiguredWithApplication");
        SharedPreferences sharedPreferences =
                this.getSharedPreferences(getString(R.string.persistent_storage_name), Context.MODE_PRIVATE);

        if(sharedPreferences.contains(getString(R.string.persistent_storage_user_username)))
            username = sharedPreferences.getString(getString(R.string.persistent_storage_user_username), null);
        else
            return false;

        Log.d("TICKET_MANAGEMENT","TicketsActivity:userConfiguredWithApplication: Contained username.");

        if(sharedPreferences.contains(getString(R.string.persistent_storage_user_token)))
            userToken = sharedPreferences.getString(getString(R.string.persistent_storage_user_token), null);
        else
            return false;

        Log.d("TICKET_MANAGEMENT","TicketsActivity:userConfiguredWithApplication: Contained userToken.");

        return true;
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
}
