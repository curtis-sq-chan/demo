package com.example.selfieunlock;

import android.graphics.Bitmap;
import android.graphics.PointF;
import android.media.FaceDetector;
import android.media.FaceDetector.Face;

public class FaceCropperFactory {
	public static Bitmap CropFaceFromImage( Bitmap bMap ) throws NoFaceFoundException, FaceNotCenteredException {
    	// find the face
    	FaceDetector detector = new FaceDetector( bMap.getWidth(), bMap.getHeight(), 1 );
    	Face[] faces = new Face[ 1 ];
    	if( detector.findFaces( bMap, faces) <= 0 )
    	{
    		// couldn't find any faces
    		throw new NoFaceFoundException();
    	}
    	if( faces[0].confidence() < 0.3 )
    	{
    		// couldn't find any faces
    		return null;
    	}
    	
    	// figure out dimensions        	
    	PointF myMidPoint = new PointF();
    	faces[0].getMidPoint( myMidPoint );
    	int eyeDistance = (int)faces[0].eyesDistance();
    	int calculatedLeftBoundary = (int)(myMidPoint.x - 2*eyeDistance);
    	int calculatedRightBoundary = (int)(myMidPoint.x + 2*eyeDistance);
    	int calculatedTopBoudary = (int)(myMidPoint.y - 2*eyeDistance);
    	int calculatedBottomBoudary = (int)(myMidPoint.y + 2*eyeDistance);
    	
    	int xOffset = calculatedLeftBoundary;
    	int xEnd = calculatedRightBoundary;
    	int yOffset = calculatedTopBoudary;
    	int yEnd = calculatedBottomBoudary;
    	if( calculatedLeftBoundary < 0 )
    	{
    		throw new FaceNotCenteredException();
    	}
    	if( calculatedRightBoundary >= bMap.getWidth() )
    	{
    		throw new FaceNotCenteredException();
    	}
    	if( calculatedTopBoudary < 0 )
    	{
    		throw new FaceNotCenteredException();
    	}
    	if( calculatedRightBoundary >= bMap.getHeight() )
    	{
    		throw new FaceNotCenteredException();
    	}
    	
    	// crop the face
    	Bitmap cropped = Bitmap.createBitmap( bMap, xOffset, yOffset, (xEnd-xOffset+1), (yEnd-yOffset+1) );
    	
    	return cropped;
	}
}
