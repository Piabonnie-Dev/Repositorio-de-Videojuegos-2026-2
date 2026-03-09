#include <Windows.h>
#include <GL/glut.h>
#include <math.h>
#include <stdio.h>


#define PI 3.1415926535898 

double xpos, ypos, ydir, xdir;         // x and y position for house to be drawn
double sx, sy, squash;          // xy scale factors
double rot, rdir;             // rotation
double ball_speed; // velocidad de la pelota
double p1x, p1y; // jugador izquierdo
double p2x, p2y; // jugador derecho
double paddleW = 4.0; //anchura de mis paletas x
double paddleH = 24; //altura de mis paletas y
double paddleSpeed = 10.0; //velocidad de las paletas 

int scoreP1 = 0;
int scoreP2 = 0;
bool gameStarted = false;


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

void resetBall(int direction)
{
    xpos = 80.0;
    ypos = 60.0;
    xdir = direction;
    ydir = 1.0;
    p1y = (120.0 - paddleH) / 2.0;
    p2y = (120.0 - paddleH) / 2.0;
    gameStarted = false;
}

// Draw the ball, centered at the origin
void draw_ball() {
    glColor3f(0, 0., 0);
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

void draw_scene() {

    glBegin(GL_QUADS);
    glVertex2f(0.0f, 0.0f);



}

void keyboard(unsigned char key, int x, int y)
{
    switch (key)
    {
    case 'w':  //si aprieta la tecla w o W mayuscula, la paleta subira 
    case 'W':
        if (gameStarted)
        {
            p1y += paddleSpeed;
        }
        break;


    case 's':// si aprieta s o S, la paleta bajara
    case 'S':
        if (gameStarted)
        {
            p1y -= paddleSpeed;
        }
        break;

    case ' ':
        gameStarted = true;
        break;

    case 27:
        exit(0);

    }
    if (p1y < 0.0)p1y = 0.0;  //si la paleta es menor a la posicion de 0.0, se mantendra quieta en el borde 0.0
    if (p1y + paddleH > 120.0) p1y = 120.0 - paddleH; // 



}


void specialKeys(int key, int x, int y) {


    switch (key)
    {

    case GLUT_KEY_UP:
        if (gameStarted)
        {
            p2y += paddleSpeed;
        }
        break;


    case GLUT_KEY_DOWN:
        if (gameStarted)
        {
            p2y -= paddleSpeed;
        }
        break;

    case 27:

        exit(0);


    }

    if (p2y + paddleH > 120.0) p2y = 120.0 - paddleH;
    if (p2y < 0.0) p2y = 0.0;
}



double clampYDir(double value)
{
    if (value > 1.0) return 1.0;
    if (value < -1.0) return -1.0;
    return value;
}

void drawText(float x, float y, const char* text)
{
    glRasterPos2f(x, y);
    while (*text)
    {
        glutBitmapCharacter(GLUT_BITMAP_HELVETICA_18, *text);
        text++;
    }
}

void drawHUD()
{
    char marcador[100];
    sprintf_s(marcador, "Jugador 1: %d    Jugador 2: %d", scoreP1, scoreP2);

    glColor3f(0.0f, 0.0f, 0.0f);
    glLoadIdentity();
    drawText(52.0f, 114.0f, marcador);

    if (!gameStarted)
    {
        drawText(43.0f, 108.0f, "Presiona ESPACIO para comenzar");
    }
}

void Display(void)
{
    //clear all pixels with the specified clear color
    glClear(GL_COLOR_BUFFER_BIT);
    // 160 is max X value in our world

    //Movimiento Diagonal
    sx = 1.0;
    sy = 1.0;

    if (gameStarted)
    {
        //Mover la pelota en X y en Y  
        xpos += xdir * ball_speed;
        ypos += ydir * ball_speed;


        // El techo de mi ventana, con este if hago que baje la pelota si choca en el techo
        if (ypos >= 120.0 - RadiusOfBall)
        {
            ypos = 120 - RadiusOfBall;
            ydir = -fabs(ydir);

        }
        // El suelo, hago que sube la pelota
        else if (ypos <= RadiusOfBall)
        {
            ypos = RadiusOfBall;
            ydir = fabs(ydir);
        }

        // La pared derecha ahora da punto al jugador izquierdo
        if (xpos >= 160 - RadiusOfBall) {
            scoreP1++;
            resetBall(-1);
        }
        // La pared izquierda ahora da punto al jugador derecho
        else if (xpos <= RadiusOfBall) {
            scoreP2++;
            resetBall(1);
        }
        else
        {
            // Colision de paletas
            bool hitP1 = // paleta izquierda
                (xpos - RadiusOfBall <= p1x + paddleW) &&
                (xpos + RadiusOfBall >= p1x) &&
                (ypos + RadiusOfBall >= p1y) &&
                (ypos - RadiusOfBall <= p1y + paddleH);

            if (hitP1 && xdir < 0)
            {
                xpos = p1x + paddleW + RadiusOfBall;
                xdir = 1;

                double paddleCenter = p1y + paddleH / 2.0;
                double hitOffset = ypos - paddleCenter;
                ydir = clampYDir(hitOffset / (paddleH / 2.0));
            }

            bool hitP2 =
                (xpos + RadiusOfBall >= p2x) &&
                (xpos - RadiusOfBall <= p2x + paddleW) &&
                (ypos + RadiusOfBall >= p2y) &&
                (ypos - RadiusOfBall <= p2y + paddleH);

            if (hitP2 && xdir > 0) {
                xpos = p2x - RadiusOfBall;
                xdir = -1;

                double paddleCenter = p2y + paddleH / 2.0;
                double hitOffset = ypos - paddleCenter;
                ydir = clampYDir(hitOffset / (paddleH / 2.0));
            }
        }
    }




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
    glColor3f(0.0f, 0.0f, 9.0f); //color 
    T[12] = (GLfloat)p1x; // indicando la matriz que dibuje mi paleta en el eje horizontal
    T[13] = (GLfloat)p1y; // y vertical
    glLoadMatrixf(T);
    draw_paddle(); // dibuja las paletas

    // =====paleta derecha=====
    glColor3f(6.0f, 0.0f, 0.0f); // color 
    T[12] = (GLfloat)p2x;// lo mismo pero con mi paleta derecha
    T[13] = (GLfloat)p2y;
    glLoadMatrixf(T);
    draw_paddle();


    drawHUD();


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
    glClearColor(0.0, 0.8, 1.0, 1.0);
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
    p1y = (120.0 - paddleH) / 2.0;
    p2x = 160.0 - 6.0 - paddleW;
    p2y = (120.0 - paddleH) / 2.0;


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
    glutKeyboardFunc(keyboard);
    glutSpecialFunc(specialKeys);
    glutReshapeFunc(reshape);
    glutTimerFunc(16, Timer, 0);
    glutMainLoop();

    return 1;
}