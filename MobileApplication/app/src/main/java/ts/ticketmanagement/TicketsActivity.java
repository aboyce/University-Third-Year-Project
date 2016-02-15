package ts.ticketmanagement;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;

public class TicketsActivity extends AppCompatActivity {

    static final int LOGIN_FROM_TICKETS = 0;

    private String username;
    private String userToken;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        // If the phone cannot find a valid username/userToken, then assume they are
        // not 'logged in', so send them to the Login Page.
        if(!userConfiguredWithApplication()){
            Intent loginIntent = new Intent(this, LoginActivity.class);
            startActivityForResult(loginIntent, LOGIN_FROM_TICKETS);
        }

        // Load up the Tickets Page
        setContentView(R.layout.activity_tickets);
    }

    @Override
    protected void onActivityResult(int pRequestCode, int pResultCode, Intent pData){

        if(pRequestCode == LOGIN_FROM_TICKETS){
            if (pResultCode == RESULT_OK){
                if(pData.hasExtra("login_username")){
                    username = pData.getStringExtra("login_username");
                }
                // TODO: if the user is logged in...
            }
            else if(pResultCode == RESULT_CANCELED){
                // TODO: if the user is not logged in...
            }
        }
    }


    private boolean userConfiguredWithApplication()    {
        SharedPreferences sharedPreferences = getPreferences(Context.MODE_PRIVATE);

        if(sharedPreferences.contains("user_username"))
            username = getString(R.string.user_username);
        else
            return false;


        if(sharedPreferences.contains("user_Token"))
            userToken = getString(R.string.user_token);
        else
            return false;

        return true;
    }
}
