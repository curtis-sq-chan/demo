package com.example.selfieunlock;

import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.InputStream;

import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;
import android.app.Activity;
import android.app.AlertDialog;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Bitmap.Config;

public class MainActivity extends Activity {

	private static final int REQUEST_CODE = 1;
	
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.fragment_main);
        
        Button button = (Button) findViewById(R.id.buttonPicture);
        button.setEnabled( false );
        
        ImageView imageView = (ImageView) findViewById(R.id.result);
        imageView.setImageResource(android.R.color.transparent);
    }

    /**
     * A placeholder fragment containing a simple view.
     */
    /*public static class PlaceholderFragment extends Fragment {

        public PlaceholderFragment() {
        }

        @Override
        public View onCreateView(LayoutInflater inflater, ViewGroup container,
                Bundle savedInstanceState) {
            View rootView = inflater.inflate(R.layout.fragment_main, container, false);
            return rootView;
        }
    }*/
    
    /** Called when the user clicks the Send button */
    public void takePicture(View view) {
    	Intent intent = new Intent(this, TakePictureActivity.class);
    	startActivity( intent );
    }
    
    public void pickImage(View View) {
        Intent intent = new Intent();
        intent.setType("image/*");
        intent.setAction(Intent.ACTION_GET_CONTENT);
        intent.addCategory(Intent.CATEGORY_OPENABLE);
        startActivityForResult(intent, REQUEST_CODE );
    }
    
    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        
    	if( requestCode != REQUEST_CODE || resultCode != Activity.RESULT_OK )
    	{
    		return;
    	}
    	
    	Bitmap bMap = null;
		try {
			InputStream stream = getContentResolver().openInputStream( data.getData());
	    	BitmapFactory.Options options = new BitmapFactory.Options();
	    	options.inPreferredConfig = Config.RGB_565;
	    	bMap = BitmapFactory.decodeStream(stream, null, options );
			stream.close();
		} catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return;
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return;
		}
    	
    	// crop the face
        Bitmap cropped = null;
        try{
            cropped = FaceCropperFactory.CropFaceFromImage( bMap );
        }
        catch( FaceNotCenteredException e )
        {
        	new AlertDialog.Builder(this).setTitle("Oops.").setMessage("Please make sure face it centered.").show();
        	bMap.recycle();
        	return;
        }
        catch( NoFaceFoundException e )
        {
        	new AlertDialog.Builder(this).setTitle("Oops.").setMessage("A face wasn't detected.").show();
        	bMap.recycle();
        	return;
        }
        
    	bMap.recycle();
        ImageView imageView = (ImageView) findViewById(R.id.result);
        imageView.setImageBitmap( cropped );
        
        Button button = (Button) findViewById(R.id.loadPicture);
        button.setEnabled( false );
        
        // calculate the DFT
        new CalculateDFTPictureTask().execute( cropped );
        
        super.onActivityResult(requestCode, resultCode, data);
    }
    
    private class CalculateDFTPictureTask extends CalculateDFTTask {
    	
        /** The system calls this to perform work in the UI thread and delivers
         * the result from doInBackground() */
       protected void onPostExecute(Double result) {
           TextView output = (TextView) findViewById(R.id.dftOuput);      
           output.setText( String.format("%.2f", result) );
           
           Button pictureButton = (Button) findViewById(R.id.buttonPicture);
           pictureButton.setEnabled( true );
           
           Button loadbutton = (Button) findViewById(R.id.loadPicture);
           loadbutton.setEnabled( true );
       }
    }

}
