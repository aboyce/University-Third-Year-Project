package ts.ticketmanagement;

import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;

public class ActivityBase extends AppCompatActivity {

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
            startActivity(new Intent(this, SettingsActivity.class));;
        }
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


