package ts.ticketmanagement;

import android.content.Context;
import android.content.SharedPreferences;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;

public class Login extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);
    }

    private String getUsernameFromPhone(){
        //SharedPreferences preferences = getPreferences(Context.MODE_PRIVATE);
        return getResources().getString(R.string.user_username);
    }

    private Boolean setUsernameToPhone(String username){
        SharedPreferences preferences = getPreferences(Context.MODE_PRIVATE);
        SharedPreferences.Editor editor = preferences.edit();
        editor.putString(getString(R.string.user_username), username);
        editor.commit();
        return true;
    }

    private String getUserTokenFromPhone(){
        //SharedPreferences preferences = getPreferences(Context.MODE_PRIVATE);
        return getResources().getString(R.string.user_userToken);
    }

    private Boolean setUserTokenToPhone(String userToken){
        SharedPreferences preferences = getPreferences(Context.MODE_PRIVATE);
        SharedPreferences.Editor editor = preferences.edit();
        editor.putString(getString(R.string.user_userToken), userToken);
        editor.commit();
        return true;
    }

    //http://developer.android.com/training/basics/data-storage/shared-preferences.html
}
