package com.example.selfieunlock;

import android.graphics.Bitmap;
import android.graphics.Color;
import android.os.AsyncTask;
import android.util.Log;


public class CalculateDFTTask extends AsyncTask<Bitmap, Void, Double> {
    /** The system calls this to perform work in a worker thread and
      * delivers it the parameters given to AsyncTask.execute() */
    protected Double doInBackground(Bitmap... bitmap) {
        
    	// Scale the BM of bitmap
    	Bitmap scaledBM = scaledBM( bitmap[0] );
    	
    	// working matrix
    	double[][] realMatrix = new double[ scaledBM.getHeight() ][ scaledBM.getWidth() ];
    	double[][] imagMatrix = new double[ scaledBM.getHeight() ][ scaledBM.getWidth() ];
    	
    	// process rows
    	Log.d( "Photo", "Process Rows - Size: " + scaledBM.getHeight() );
		double[] rowImagInput = new double[ scaledBM.getWidth() ];
    	for ( int i = 0; i < scaledBM.getHeight(); ++i )
    	{
    		// get the row and transform it and store it
    		double[] realInput = new double[ scaledBM.getWidth() ];
    		for( int colorIndex = 0; colorIndex < scaledBM.getWidth(); ++colorIndex  )
    		{
    			int packedColor = scaledBM.getPixel( colorIndex, i );
    			realInput[ colorIndex ] = (0.2125 * Color.red( packedColor ) ) + (0.7154 * Color.green( packedColor ) ) + (0.0721 * Color.blue( packedColor ) );
    		}
    		
    		dft( realInput, rowImagInput, realMatrix[ i ], imagMatrix[ i ] );
    	}
    	
    	// process columns
    	Log.d( "Photo", "Process Columns - Size: " + scaledBM.getWidth() );
    	for ( int i = 0; i < scaledBM.getWidth(); ++i )
    	{        		
    		// get the row and transform it and store it
    		double[] realInput = GetColumn( realMatrix, i );
    		double[] imagInput = GetColumn( imagMatrix, i );
    		
    		double[] realOutput = new double[ scaledBM.getHeight() ];
    		double[] imagOuput = new double[ scaledBM.getHeight() ];
    		
    		dft( realInput, imagInput, realOutput, imagOuput );
    		
    		// Save the results to the working matrix
    		SetColumn( realMatrix, i, realInput );
    		SetColumn( imagMatrix, i, imagInput );
    	}
    	
    	double resultValue = 0;
    	for( int i = 0; i < 5; ++i )
    	{
    		for( int j = 0; j < 5-i; ++j )
    		{
    			resultValue = Math.sqrt( Math.pow( realMatrix[i][j], 2.0 ) + Math.pow( imagMatrix[i][j], 2.0 ) );
    		}
    	}
    	
    	return resultValue/Math.sqrt( scaledBM.getHeight() * scaledBM.getWidth() ); // normalize
    }
    
    private void dft(double[] inreal, double[] inimag, double[] outreal, double[] outimag) {
        int n = inreal.length;
        for (int k = 0; k < n; k++) {  // For each output element
            double sumreal = 0;
            double sumimag = 0;
            for (int t = 0; t < n; t++) {  // For each input element
                double angle = 2 * Math.PI * t * k / n;
                sumreal +=  inreal[t] * Math.cos(angle) + inimag[t] * Math.sin(angle);
                sumimag += -inreal[t] * Math.sin(angle) + inimag[t] * Math.cos(angle);
            }
            outreal[k] = sumreal;
            outimag[k] = sumimag;
        }
    }
    
    private double[] GetColumn( double[][] input, int column )
    {
    	double [] columnArray = new double[ input.length ];
    	
    	for( int i = 0; i < input.length; ++i )
    	{
    		columnArray[ i ] = input[ i ][ column ];
    	}
    	
    	return columnArray;
    }
    
    private void SetColumn( double[][] input, int column, double[] newColumn )
    {
    	for( int i = 0; i < input.length; ++i )
    	{
    		input[ i ][ column ] = newColumn[ i ];
    	}
    }
    
    private Bitmap scaledBM(Bitmap bmpOriginal)
    {        
        int width, height;
        height = bmpOriginal.getHeight();
        width = bmpOriginal.getWidth();
        
        while( height > 200 )
        {
        	height = height/2;
        	width = width/2;
        }
        	
        
        Bitmap bmpGrayscale = Bitmap.createScaledBitmap( bmpOriginal, 256, 256, true );
        return bmpGrayscale;
    }
}
