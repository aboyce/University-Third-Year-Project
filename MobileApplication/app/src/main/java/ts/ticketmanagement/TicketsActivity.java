package ts.ticketmanagement;

import android.content.Context;
import android.content.SharedPreferences;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;

public class TicketsActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_tickets);

        boolean appConfigured = userConfiguredWithApplication();

    }


    private boolean userConfiguredWithApplication()
    {
        SharedPreferences sharedPreferences = getPreferences(Context.MODE_PRIVATE);



        return false;
    }
}
