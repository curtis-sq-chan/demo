package com.example.selfieunlock;

import java.io.ByteArrayOutputStream;
import java.io.FileOutputStream;
import java.io.IOException;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.Context;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.Matrix;
import android.graphics.Bitmap.Config;
import android.graphics.BitmapFactory;
import android.hardware.Camera;
import android.hardware.Camera.PictureCallback;
import android.os.Bundle;
import android.view.View;
import android.widget.FrameLayout;

public class TakePictureActivity extends Activity {

    private Camera mCamera;
    private CameraPreview mPreview;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.fragment_take_picture);

        // Create an instance of Camera
        mCamera = getCameraInstance();

        // Create our Preview view and set it as the content of our activity.
        mPreview = new CameraPreview(this, mCamera);
        FrameLayout preview = (FrameLayout) findViewById(R.id.camera_preview);
        preview.addView(mPreview);
    }
    
    /** A safe way to get an instance of the Camera object. */
    public static Camera getCameraInstance(){
        Camera c = null;
        try {
            c = Camera.open(1); // attempt to get a Camera instance
            c.setDisplayOrientation(90);
        }
        catch (Exception e){
            // Camera is not available (in use or does not exist)
        }
        return c; // returns null if camera is unavailable
    }
    
    @Override
    protected void onPause() {
        super.onPause();
        releaseCamera();              // release the camera immediately on pause event
    }

    private void releaseCamera(){
        if (mCamera != null){
            mCamera.release();        // release the camera for other applications
            mCamera = null;
        }
    }
    
    /** Called when the user clicks the unlock button */
    public void takePicture(View view) {
    	
    	mCamera.takePicture(null, null, new SelfiePictureCallback( this ) );
    }
    
    
    private class SelfiePictureCallback implements PictureCallback {
        
    	private Context m_context;
    	
    	public SelfiePictureCallback( Context context )
    	{
    		m_context = context;
    	}
    	
    	@Override
        public void onPictureTaken(byte[] data, Camera camera) {            
            
        	BitmapFactory.Options options = new BitmapFactory.Options();
        	options.inPreferredConfig = Config.RGB_565;
        	Bitmap rawBMap = BitmapFactory.decodeByteArray(data, 0, data.length, options );
        	
            // turn the image 90 degrees
            Matrix mat = new Matrix();
            mat.postRotate( -90 );        
            Bitmap bMap = Bitmap.createBitmap(rawBMap, 0, 0, rawBMap.getWidth(), rawBMap.getHeight(), mat, true);
            rawBMap.recycle();
        	
        	// crop the face
            Bitmap cropped = null;
            try{
                cropped = FaceCropperFactory.CropFaceFromImage( bMap );
            }
            catch( FaceNotCenteredException e )
            {
            	new AlertDialog.Builder( m_context ).setTitle("Oops.").setMessage("Please make sure face it centered.").show();
            	bMap.recycle();
            	return;
            }
            catch( NoFaceFoundException e )
            {
            	new AlertDialog.Builder( m_context ).setTitle("Oops.").setMessage("A face wasn't detected.").show();
            	bMap.recycle();
            	return;
            }

        	bMap.recycle();
        	ByteArrayOutputStream stream = new ByteArrayOutputStream();
        	cropped.compress(Bitmap.CompressFormat.PNG, 100, stream);
        	byte[] byteArray = stream.toByteArray();
        	cropped.recycle();
        	
        	// store it
        	String fileName = "tempIMG.png";
            try {
                FileOutputStream fileOutStream = openFileOutput(fileName, MODE_PRIVATE);
                fileOutStream.write( byteArray );
                fileOutStream.close();
            } catch (IOException ioe) {
                ioe.printStackTrace();
            }           
            
        	Intent intent = new Intent( TakePictureActivity.this.getBaseContext(), ManipulateActivity.class);
        	intent.putExtra(CAMERA_DATA, fileName );
        	startActivity( intent );
        }
    }
    
    public final static String CAMERA_DATA = "com.example.SelfieUnlock.CAMERADATA";
}
