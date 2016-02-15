package ts.ticketmanagement;

import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.TextView;

public class LoginActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);
    }

    public void authenticateOnClick(View pView){
        TextView txtUsername = (TextView)findViewById(R.id.txtUsername);
        String username = txtUsername.getText().toString();

        if(username.isEmpty()){
            showMessageBox("Username Required", "Please enter your username.");
            return;
        }

        String userToken = tryGetUserToken();

        if(userToken.isEmpty()){
            showMessageBox("Error Getting Token", "An error has occurred trying to get your" +
                    " user token, please check the configuration and try again");
            return;
        }

        if(storeCredentials(username, userToken)){
            Intent intentWithData = new Intent();
            intentWithData.putExtra("login_username", username);
            setResult(RESULT_OK, intentWithData);
        } else
            setResult(RESULT_CANCELED);
    }

    private String tryGetUserToken()
    {
        return "valid user token";
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
            e.fillInStackTrace();
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
}
