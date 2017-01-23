package com.example.selfieunlock;

import java.io.File;

import android.app.Activity;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.os.AsyncTask;
import android.os.Bundle;
import android.renderscript.Allocation;
import android.renderscript.Element;
import android.renderscript.RenderScript;
import android.renderscript.ScriptIntrinsicBlur;
import android.util.Log;
import android.view.View;
import android.widget.ImageView;
import android.widget.TextView;

public class ManipulateActivity extends Activity {
	
	File m_filePath;
	
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.fragment_manipulate);

        Intent intent = getIntent();
        String photoPath = intent.getStringExtra( TakePictureActivity.CAMERA_DATA );
        Log.d( "Photo", "PhotoPatch: " + photoPath );
        
        m_filePath = getFileStreamPath(photoPath);
        
        setOriginal();
    }
    
    public void setOriginal(View view) {
    	setOriginal();
    }
    
    private void setOriginal() {
        // Create our Preview view and set it as the content of our activity.
        ImageView preview = (ImageView) findViewById(R.id.final_picture);
        Bitmap original = BitmapFactory.decodeFile( m_filePath.getAbsolutePath());
        preview.setImageBitmap( original );
        new CalculateDFTPicutreTask().execute( original );
    }
    
    public void setBlur(View view) {
        Bitmap original = BitmapFactory.decodeFile( m_filePath.getAbsolutePath());
    	new BlurTask().execute( original );
    }
    
    private class BlurTask extends AsyncTask<Bitmap, Void, Bitmap> {
        /** The system calls this to perform work in a worker thread and
          * delivers it the parameters given to AsyncTask.execute() */
        protected Bitmap doInBackground(Bitmap... bitmap) {
            
        	Bitmap blurredBitmap = Bitmap.createBitmap( bitmap[0] );
        	
            RenderScript rs = RenderScript.create( ManipulateActivity.this.getBaseContext() );
            Allocation input = Allocation.createFromBitmap( rs, bitmap[0], Allocation.MipmapControl.MIPMAP_FULL, Allocation.USAGE_SCRIPT );
            Allocation output = Allocation.createTyped( rs, input.getType() );
            
            ScriptIntrinsicBlur script = ScriptIntrinsicBlur.create( rs, Element.U8_4(rs) );
            script.setInput( input );
            script.setRadius( 10 );
            script.forEach( output );
            output.copyTo( blurredBitmap );
        	return blurredBitmap;
        }
        
        /** The system calls this to perform work in the UI thread and delivers
          * the result from doInBackground() */
        protected void onPostExecute(Bitmap result) {
            ImageView preview = (ImageView) findViewById(R.id.final_picture);      
            preview.setImageBitmap( result );
            
            new CalculateDFTPicutreTask().execute( result );
        }
    }
    
    private class CalculateDFTPicutreTask extends CalculateDFTTask {
    	
        /** The system calls this to perform work in the UI thread and delivers
         * the result from doInBackground() */
       protected void onPostExecute(Double result) {
           TextView output = (TextView) findViewById(R.id.dftOuput);      
           output.setText( String.format("%.2f", result) );
       }
    }
}
