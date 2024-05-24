using Raylib_cs;
using RTInOneWeekend;
using System.Diagnostics;
using System.Numerics;

float aspect_ratio = 16.0f / 9.0f;
int image_width = 600;
//double aspect_ratio = 3.0 / 2.0;
//int image_width = 400;
int image_height = Convert.ToInt32((image_width / aspect_ratio));
//int samples_per_pixel = 100;
int samples_per_pixel = 100;
//int max_depth = 50;
int max_depth = 10;

Raylib.InitWindow(image_width, image_height, "RT in One Weekend");
//Raylib.ToggleFullscreen();

int renderWidth = Raylib.GetRenderWidth();
int renderHeight = Raylib.GetRenderHeight();

Byte[] buffer = new byte[Raylib.GetRenderWidth() * Raylib.GetRenderHeight() * 4];

long frameCount = 0;

Stopwatch sw = new Stopwatch();
double frameTime = sw.Elapsed.TotalMilliseconds;

long totalRays = 0;


//Image

//World
var R = MathF.Cos(Util.pi / 4);
hittable_list world = new hittable_list();

var material_ground = new Lambertian(new Vector3(0.8f, 0.8f, 0.0f));
var material_center = new Lambertian(new Vector3(0.1f, 0.2f, 0.5f));
var material_left = new Dieletric(1.5f);
var material_right = new Metal(new Vector3(0.8f, 0.6f, 0.2f), 0.0f);

world.add(new Sphere(new Vector3(0.0f, -100.5f, -1.0f), 100.0f, material_ground));
world.add(new Sphere(new Vector3(0.0f, 0.0f, -1.0f), 0.5f, material_center));
world.add(new Sphere(new Vector3(-1.0f, 0.0f, -1.0f), 0.5f, material_left));
world.add(new Sphere(new Vector3(-1.0f, 0.0f, -1.0f), -0.4f, material_left));
world.add(new Sphere(new Vector3(1.0f, 0.0f, -1.0f), 0.5f, material_right));

//hittable_list world = random_scene();

//Camera
//Vec3 lookfrom = new Vec3(3, 3, 2);
//Vec3 lookat = new Vec3(0, 0, -1);
Vector3 lookfrom = new Vector3(13, 2, 3);
Vector3 lookat = new Vector3(0, 0, 0);
Vector3 vup = new Vector3(0, 1, 0);
//double dist_to_focus = (lookfrom - lookat).length();
//double aperture = 2.0;
float dist_to_focus = 10.0f;
float aperture = 0.1f;

Camera cam = new Camera(lookfrom, lookat, vup, 20, aspect_ratio, aperture, dist_to_focus);

while (!Raylib.WindowShouldClose())
{
    sw.Restart();

    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.White);

    render();

    Raylib.EndDrawing();

    sw.Stop();
    frameTime = sw.Elapsed.TotalMilliseconds;

    if (frameCount % 10 == 0) //a cada 50 frames mostra o ultimo frametime
    {
        Raylib.SetWindowTitle(string.Format("{0}ms. FPS: {1}", frameTime, 1000.0f / frameTime));
    }

    frameCount++;
}

Raylib.CloseWindow();

void render()
{
    for (int x = 0; x < renderWidth; x++)
    //Parallel.For(0, renderWidth, new ParallelOptions { MaxDegreeOfParallelism = 6 }, x =>
    {
        for (int ty = 0; ty < renderHeight; ty++)
        //Parallel.For(0, renderHeight, ty =>
        {
            //o y no Raylib comeca de cima pra baixo. no GLSL comeca de baixo pra cima. entao trocamos aqui.

            int y = Math.Abs(ty - renderHeight);

            Vector3 pixel_color = new Vector3(0, 0, 0);
            for (int s = 0; s < samples_per_pixel; ++s)
            {
                float u = (x + Util.random_float()) / (image_width - 1);
                float v = (y + Util.random_float()) / (image_height - 1);
                RTInOneWeekend.Ray r = cam.get_ray(u, v);
                pixel_color += ray_color(r, world, max_depth);
            }

            write_color(x, ty, pixel_color, samples_per_pixel);
        }
        //});
    }
    //});

    Image i2 = new Image
    {
        Format = PixelFormat.UncompressedR8G8B8A8,
        Width = renderWidth,
        Height = renderHeight,
        Mipmaps = 1
    };

    Texture2D t2 = Raylib.LoadTextureFromImage(i2);

    unsafe
    {
        fixed (byte* bPtr = &buffer[0])
        {
            Raylib.UpdateTexture(t2, bPtr);
            Raylib.DrawTexture(t2, 0, 0, Color.White);
        }
    }
}


void write_color(int x, int y, Vector3 pixel_color, int samples_per_pixel)
{
    float r = pixel_color.X;
    float g = pixel_color.Y;
    float b = pixel_color.Z;

    //Divide the color by the number of  samples and gamma-correct for gamma=2.0.
    float scale = 1.0f / samples_per_pixel;
    r = MathF.Sqrt(scale * r);
    g = MathF.Sqrt(scale * g);
    b = MathF.Sqrt(scale * b);

    byte ir = (byte)(255 * Util.clamp(r, 0.0f, 0.999f));
    byte ig = (byte)(255 * Util.clamp(g, 0.0f, 0.999f));
    byte ib = (byte)(255 * Util.clamp(b, 0.0f, 0.999f));

    int idx = (y * renderWidth + x) * 4;

    buffer[idx] = ir;
    buffer[idx + 1] = ig;
    buffer[idx + 2] = ib;
    buffer[idx + 3] = 255;
}

Vector3 unit_vector(Vector3 v)
{   
    return v / v.Length();
}

Vector3 ray_color(RTInOneWeekend.Ray r, Hittable world, int depth)
{
    totalRays++;
    hit_record rec = new hit_record();

    // If we've exceeded the ray bounce limit, no more light is gathered.
    if (depth <= 0)
        return new Vector3(0, 0, 0);

    if (world.hit(r, 0.001f, Util.infinity, ref rec))
    {
        RTInOneWeekend.Ray scattered = null;

        //FIXME: nao sei se vai funcionar. Antes usava a classe Vec3 e deixava como nulo aqui.
        Vector3 attenuation = Vector3.One;

        if (rec.mat_ptr.scatter(r, rec, ref attenuation, ref scattered))
            return attenuation * ray_color(scattered, world, depth - 1);

        return new Vector3(0, 0, 0);
    }

    Vector3 unit_direction = unit_vector(r.direction());
    float t = 0.5f * (unit_direction.Y + 1.0f);
    return (1.0f - t) * (new Vector3(1.0f, 1.0f, 1.0f)) + t * (new Vector3(0.5f, 0.7f, 1.0f));
}