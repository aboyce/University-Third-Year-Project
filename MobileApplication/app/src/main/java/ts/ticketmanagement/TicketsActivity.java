package ts.ticketmanagement;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;


public class TicketsActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        // Load up the Tickets Page
        setContentView(R.layout.activity_tickets);
    }
}
