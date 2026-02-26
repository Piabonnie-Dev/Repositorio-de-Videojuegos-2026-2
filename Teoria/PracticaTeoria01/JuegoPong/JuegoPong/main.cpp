#include <Windows.h>
#include <GL/glut.h>
#include <math.h>
#include <stdio.h>


#define PI 3.1415926535898 

double xpos, ypos, ydir, xdir;         // x and y position for house to be drawn
double sx, sy, squash;          // xy scale factors
double rot, rdir;             // rotation
double ball_speed;
double p1x, p1y; // jugador izquierdo
double p2x, p2y; // jugador derecho
double paddleW = 4.0;
double paddleH = 24.0;
double paddleSpeed = 2.0;


GLfloat T1[16] = { 1.,0.,0.,0.,\
				  0.,1.,0.,0.,\
				  0.,0.,1.,0.,\
				  0.,0.,0.,1. };
GLfloat S[16] = { 1.,0.,0.,0.,\
				 0.,1.,0.,0.,\
				 0.,0.,1.,0.,\
				 0.,0.,0.,1. };
GLfloat T[16] = { 1.,0.,0.,0.,\
				 0., 1., 0., 0.,\
				 0.,0.,1.,0.,\
				 0.,0.,0.,1. };



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

GLfloat RadiusOfBall = 2.;
// Draw the ball, centered at the origin
void draw_ball() {
	glColor3f(1.3, 1.2, 0.4);
	MyCircle2f(0., 0., RadiusOfBall);

}

void draw_paddle() {

	glBegin(GL_QUADS);
	glVertex2f(0.0f, 0.0f);
	glVertex2f((GLfloat)paddleW, 0.0f);
	glVertex2f((GLfloat)paddleW, (GLfloat)paddleH);
	glVertex2f(0.0f, (GLfloat)paddleH);
	glEnd();

}

void Display(void)
{
	// swap the buffers
	

	//clear all pixels with the specified clear color
	glClear(GL_COLOR_BUFFER_BIT);
	// 160 is max X value in our world
	
	//Movimiento Diagonal
	sx = 1.0;
	sy = 1.0; 

	 //Mover la pelota en X y en Y  
	xpos += xdir * ball_speed;
	ypos += ydir * ball_speed;

	
	// El techo de mi ventana, con este if hago que baje la pelota si choca en el techo
	if (ypos >= 120.0 - RadiusOfBall)
	{
		ypos = 120 - RadiusOfBall;
		ydir = -1;

	}
	// El suelo, hago que sube la pelota
	else if (ypos <= RadiusOfBall)
	{
		ypos = RadiusOfBall;
		ydir = 1;
	}
	// La pared derecha, hago que la pelota se pase a la izquierda
	if (xpos >= 160 - RadiusOfBall) {

		xpos = 160 - RadiusOfBall;
		xdir = -1;
	}
	// pared izquierda, hago que la pelota se pase a la derecha 
	else if (xpos <= RadiusOfBall) {
		xpos = RadiusOfBall;
		xdir = 1;

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

	//===paleta izquierda===
	glColor3f(7.0f, 0.0f, 9.0f); //color rosa
	T[12] = (GLfloat)p1x; // indicando la matriz que dibuje mi paleta en el eje horizontal
	T[13] = (GLfloat)p1y; // y vertical
	glLoadMatrixf(T);
	draw_paddle(); // dibuja las paletas

	// =====paleta derecha=====
	glColor3f(7.0f, 0.0f, 9.0f); // color rosa
	T[12] = (GLfloat)p2x;// lo mismo pero con mi paleta derecha
	T[13] = (GLfloat)p2y;
	glLoadMatrixf(T);
	draw_paddle();





	glutSwapBuffers();
	glutPostRedisplay();



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
	xpos = 80; // la posicion del centro horizontal en mi pantalla
	ypos = 60; // la posicion central vertical en mi pantalla
	xdir = 1; // Horizontal
	ydir = 1; // Vertical
	sx = 1.; 
	sy = 1.; 
	squash = 0.9;
	rot = 0;
	ball_speed = 0.03;  //velocidad de la bola
	p1x = 6.0f;
	p1y = (120.0 - paddleH) / paddleSpeed;
	p2x = 160.0 - 6.0 - paddleW;
	p2y = (120.0 - paddleH) / paddleSpeed;
}

void Timer(int value)
{
	glutPostRedisplay();
	glutTimerFunc(16, Timer, 0);
}

int main(int argc, char* argv[])
{

	glutInit(&argc, argv);
	glutInitDisplayMode(GLUT_DOUBLE | GLUT_RGB);
	glutInitWindowSize(1080, 1920);
	glutCreateWindow("Juego Pong Elliot Kenneth");
	init();
	glutDisplayFunc(Display);
	glutReshapeFunc(reshape);
	glutTimerFunc(16, Timer, 0);
	glutMainLoop();

	return 1;
}