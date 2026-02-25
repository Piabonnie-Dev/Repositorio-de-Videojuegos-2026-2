#include <Windows.h>
#include <GL/glut.h>
#include <math.h>
#include <stdio.h>


const float WORLD_W = 160.0f;
const float WORLD_H = 120.0f;


float ballX, ballY;  // el plano X & Y de mi pelota
float ballRadius = 4.0f;  //radio de mi pelota
float ballVx, ballVy; // la velocidad de mi pelota en X & Y

// =======Mis paletas ==========
float paddleW = 4.0f;  //ancho de la paleta
float paddleH = 24.0f;  // alto de la paleta
float paddleSpeed = 2.2f; //velocidad de la paleta

// =====jugador 1 y 2=====
float p1X, p1Y; //jugador 1 
float p2X, p2Y; // jugador 2

// =====puntaje=====
int score1 = 0; // puntaje del jugador 1
int score2 = 0; // puntaje del jugador 2
// ======Estado del teclado para mover las teclas W y S, Teclas " arriba y abajo"====
bool keyW = false, keyS = false; //teclas W y S
bool keyUp = false, KeyDown = false; //Teclas arriba y abajo


#define PI 3.1415926535898 
GLint circle_points = 100;
void MyCircle2f(GLfloat centerx, GLfloat centery, GLfloat radius) {
	GLint i;
	GLdouble angle;
	glBegin(GL_POLYGON);
	for (i = 0; i < circle_points; i++) {
		angle = 2 * PI * i / circle_points;
		glVertex2f(centerx + radius * cos(angle), centery + radius * sin(angle));
	}
	glEnd();
}

void drawRect(float x, float y, float w, float h) {
	
	glBegin(GL_QUADS);
	glVertex2f(x, y);
	glVertex2f(x + w, y);
	glVertex2f(x + w, y + h);
	glVertex2f(x, y + h);
	glEnd();
}


void drawBall()
{
	glColor3f(0.9f, 0.9f, 0.9f);
	MyCircle2f(ballX, ballY, ballRadius);

}

void drawPaddles() 
{
	glColor3f(0.1f, 0.1f, 0.1f);
	drawRect(p1X, p1Y, paddleW, paddleH);
	drawRect(p2X, p2Y, paddleW, paddleH);

}



void Display(void)
{
	// swap the buffers
	glutSwapBuffers();

	//clear all pixels with the specified clear color
	glClear(GL_COLOR_BUFFER_BIT);
	// 160 is max X value in our world


	  // Shape has hit the ground! Stop moving and start squashing down and then back up 
	if (ypos == RadiusOfBall && ydir == -1) {
		sy = sy * squash;

		if (sy < 0.8)
			// reached maximum suqash, now unsquash back up 
			squash = 1.1;
		else if (sy > 1.) {
			// reset squash parameters and bounce ball back upwards
			sy = 1.;
			squash = 0.9;
			ydir = 1;
		}
		sx = 1. / sy;

		// 120 is max Y value in our world

	}
	else {
		// set Y position to increment 1.5 times the direction of the bounce
		ypos += ydir * ball_speed;

		// If ball touches the top, change direction of ball downwards
		if (ypos == 120 - RadiusOfBall) {
			ydir = -1;
		}
		// If ball touches the bottom, change direction of ball upwards
		else if (ypos < RadiusOfBall)
			ydir = 1;
	}

	/*  //reset transformation state
	  glLoadIdentity();

	  // apply translation
	  glTranslatef(xpos,ypos, 0.);

	  // Translate ball back to center
	  glTranslatef(0.,-RadiusOfBall, 0.);
	  // Scale the ball about its bottom
	  glScalef(sx,sy, 1.);
	  // Translate ball up so bottom is at the origin
	  glTranslatef(0.,RadiusOfBall, 0.);
	  // draw the ball
	  draw_ball();
	*/

	//Translate the bouncing ball to its new position
	T[12] = xpos;
	T[13] = ypos;
	glLoadMatrixf(T);

	T1[13] = -RadiusOfBall;
	// Translate ball back to center
	glMultMatrixf(T1);
	S[0] = sx;
	S[5] = sy;
	// Scale the ball about its bottom
	glMultMatrixf(S);

	T1[13] = RadiusOfBall;
	// Translate ball up so bottom is at the origin

	glMultMatrixf(T1);

	draw_ball();
	



}


void reshape(int w, int h)
{
	// on reshape and on startup, keep the viewport to be the entire size of the window
	glViewport(0, 0, (GLsizei)w, (GLsizei)h);
	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();

	// keep our logical coordinate system constant
	gluOrtho2D(0.0, 160.0, 0.0, 120.0);
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();

}


void init(void) {
	//set the clear color
	glClearColor(0.0, 0.8, 0.0, 1.0);
	// initial position set to 0,0
	xpos = 80; ypos = RadiusOfBall; xdir = 1; ydir = 1;
	sx = 1.; sy = 1.; squash = 0.9;
	rot = 0;
	ball_speed = 1.5;

}


void Timer(int value) {


	glutPostRedisplay();          
	glutTimerFunc(16, Timer, 0);
}

int main(int argc, char* argv[])
{
	

	glutInit(&argc, argv);
	glutInitDisplayMode(GLUT_DOUBLE | GLUT_RGB);
	glutInitWindowSize(320, 240);
	glutCreateWindow("Bouncing Ball");
	init();
	glutDisplayFunc(Display);
	glutReshapeFunc(reshape);
	glutTimerFunc(16, Timer, 0);
	glutMainLoop();

	return 1;
}
