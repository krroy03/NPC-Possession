using UnityEngine;
using System.Collections;

public class Bezier : MonoBehaviour {

	Vector3 p0;
	Vector3 p1;
	Vector3 p2;
	Vector3 p3;
	float ti = 0f;

	Vector3 b0 = Vector3.zero;
	Vector3 b1 = Vector3.zero;
	Vector3 b2 = Vector3.zero;
	Vector3 b3 = Vector3.zero;

	float Ax, Ay, Az, Bx, By, Bz, Cx, Cy, Cz;

	//Init function 
	//v0 = 1st point v1 = handle of the 1st point v2 = handle of the 2nd point v3 = 2nd point

	public void init(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3)
	{
		this.p0 = v0;
		this.p1 = v1;
		this.p2 = v2;
		this.p3 = v3;
	}

	// 0.0 >= t <= 1.0
	public Vector3 GetPointAtTime (float t)
	{
		this.CheckConstant();
		float t2 = t * t;
		float t3 = t * t * t;
		float x = this.Ax * t3 + this.Bx * t2 + this.Cx * t + p0.x;
        float y = this.Ay * t3 + this.By * t2 + this.Cy * t + p0.y;
        float z = this.Az * t3 + this.Bz * t2 + this.Cz * t + p0.z;
        Vector3 vec = new Vector3(x,y,z);
        return(vec);
	}

	void SetConstant() {
        this.Cx = 3 * ((this.p0.x + this.p1.x) - this.p0.x);
        this.Bx = 3 * ((this.p3.x + this.p2.x) - (this.p0.x + this.p1.x)) - this.Cx;
        this.Ax = this.p3.x - this.p0.x - this.Cx - this.Bx;
       
        this.Cy = 3 * ((this.p0.y + this.p1.y) - this.p0.y);
        this.By = 3 * ((this.p3.y + this.p2.y) - (this.p0.y + this.p1.y)) - this.Cy;
        this.Ay = this.p3.y - this.p0.y - this.Cy - this.By;
 
        this.Cz = 3 * ((this.p0.z + this.p1.z) - this.p0.z);
        this.Bz = 3 * ((this.p3.z + this.p2.z) - (this.p0.z + this.p1.z)) - this.Cz;
        this.Az = this.p3.z - this.p0.z - this.Cz - this.Bz;
    }

    //check if p0,p1,p2 or p3 have changed 
	void CheckConstant() {
        if (this.p0 != this.b0 || this.p1 != this.b1 || this.p2 != this.b2 || this.p3 != this.b3) {
            this.SetConstant();
            this.b0 = this.p0;
            this.b1 = this.p1;
            this.b2 = this.p2;
            this.b3 = this.p3;
        }
    }

}
