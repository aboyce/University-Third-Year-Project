package ts.ticketmanagement;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.widget.ProgressBar;
import android.widget.Toast;


public class TicketsActivity extends ActivityBase {

    private ProgressBar progressbar;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        Log.d("TICKET_MANAGEMENT", "TicketsActivity:onCreate");
        setContentView(R.layout.activity_tickets);

        if(!tryPopulateUserCredentials("Tickets")){
            new Intent(this, MainActivity.class);
            Toast.makeText(getApplicationContext(), "No User credentials stored on Phone", Toast.LENGTH_LONG).show();
        }

    }
}
