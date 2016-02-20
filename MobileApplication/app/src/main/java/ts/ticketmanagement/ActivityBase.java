package ts.ticketmanagement;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
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
}


