import android.util.Log;
import android.view.View;

import java.io.IOException;
import java.security.KeyStore;
import java.security.KeyStoreException;
import java.security.NoSuchAlgorithmException;
import java.security.cert.CertificateException;
import java.util.ArrayList;
import java.util.Enumeration;

/**
 * Created by apboy on 16/02/2016.
 */
public class KeyStoreHelper {

    KeyStore store = null;
    Enumeration<String> aliases = null;
    ArrayList<> keyAliases;

    public void KeyStoreException(){
        try{
            store = KeyStore.getInstance("AndroidKeyStore");
            store.load(null);
        }catch (KeyStoreException e){
            Log.e("ERROR", e.getMessage(), e);
        }catch (NoSuchAlgorithmException e){
            Log.e("ERROR", e.getMessage(), e);
        } catch (CertificateException e) {
            Log.e("ERROR", e.getMessage(), e);
        } catch (IOException e) {
            Log.e("ERROR", e.getMessage(), e);
        }
    }

    public void refreshKeys(){
        keyAliases = new ArrayList<>();
        try{
            aliases = store.aliases();
            while (aliases.hasMoreElements())
                keyAliases.add(aliases.nextElement());
        }catch (KeyStoreException e){
            Log.e("ERROR", e.getMessage(), e);
        }

        // TODO: Do something with the aliases
    }

    public void createNewKeys(View view) {

    }
}


