package ts.ticketmanagement;

import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;

public class ActivityBase extends AppCompatActivity {

    protected final Integer LOGIN_FROM_MAIN = 0;

    protected String username;
    protected String userToken;

    @Override
    public boolean onCreateOptionsMenu(Menu menu){
        getMenuInflater().inflate(R.menu.ts_menu, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item){
        if(item.getItemId() == R.id.ts_menu_settings){
            startActivity(new Intent(this, SettingsActivity.class));
        }
        return true;
    }

    protected boolean tryPopulateUserCredentials(String activityName)    {
        Log.d("TICKET_MANAGEMENT", activityName + "Activity:tryPopulateUserCredentials");
        SharedPreferences sharedPreferences = this.getSharedPreferences(getString(R.string.persistent_storage_name), Context.MODE_PRIVATE);

        if(sharedPreferences.contains(getString(R.string.persistent_storage_user_username))) {
            username = sharedPreferences.getString(getString(R.string.persistent_storage_user_username), null);
            Log.d("TICKET_MANAGEMENT", activityName + "Activity:tryPopulateUserCredentials: Contained username.");
        } else return false;

        if(sharedPreferences.contains(getString(R.string.persistent_storage_user_token))){
            userToken = sharedPreferences.getString(getString(R.string.persistent_storage_user_token), null);
            Log.d("TICKET_MANAGEMENT", activityName + "Activity:tryPopulateUserCredentials: Contained userToken.");
        } else return false;

        return true;
    }

    protected void showMessageBox(String activityName, String title, String message){
        Log.d("TICKET_MANAGEMENT", activityName + "Activity:showMessageBox: Title='" + title + "' Message= " + message);
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


