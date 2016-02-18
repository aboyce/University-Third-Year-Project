package ts.ticketmanagement;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.widget.Toast;


public class TicketsActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        Log.d("TICKET_MANAGEMENT", "TicketsActivity:onCreate");
        setContentView(R.layout.activity_tickets);

        Toast.makeText(getApplicationContext(), "Logged in and authorised", Toast.LENGTH_LONG).show();
    }
}
