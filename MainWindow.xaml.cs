using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double canvasHeight;
        double canvasWidth;
        double yPosition;
        double xPosition;

        public lander moonLander { get; set; }

        public DispatcherTimer Clock { get; set; } = new DispatcherTimer();
        const float FPS = 50;

        public MainWindow()
        {
            Clock.Interval = TimeSpan.FromMilliseconds(1000f / FPS);
            Clock.Tick += Update;
            InitializeComponent();
            moonLander = new lander(this);


        }
        private void Update(object sender, EventArgs e)
        {

            // Update lander position
            moonLander.UpdateLander();

            canvasHeight = this.canvas.Height - (this.GUIlander.Height);//This ensures that the lander does not go off the screen
            canvasWidth = this.canvas.Width;

            yPosition = canvasHeight - (moonLander.y * canvasHeight / moonLander.yMax);//Convert to physical height from position
            xPosition = moonLander.x;
            // Plot point

            if (moonLander.HasLanded())
            {
                Clock.Stop();
                if (Math.Abs(moonLander.Vy)>15)
                {
                    resultLabel.Content = "MISSION FAILED";
                }
                else
                {
                    resultLabel.Content = "MISSION SUCCESS";
                }
                resultLabel.Foreground= System.Windows.Media.Brushes.White;
            }
            else
            {
                //Update the graphs
                heightVM.Points.Add(new OxyPlot.DataPoint(moonLander.t, moonLander.y));
                velocityVM.Points.Add(new OxyPlot.DataPoint(moonLander.t, moonLander.Vy));

                heightPlot.InvalidatePlot();//Tell the GUI that the height graph needs to be updated
                velocityPlot.InvalidatePlot();//Tell the GUI that the velo

                //Update the GUI lander
                Thickness newPosition = new Thickness(xPosition, yPosition, 0, 0);
                this.GUIlander.Margin = newPosition;
                //Update the warning label
                UpdateWarningLabel();
            }
        }
        private void UpdateWarningLabel()
        {
            if (Math.Abs(moonLander.Vy) > 15)
            {
                this.warningLabel.Content = "UNSAFE VELOCITY";
                this.warningLabel.Foreground = System.Windows.Media.Brushes.DarkRed;
            }
            else
            {
                this.warningLabel.Content = "SAFE VELOCITY";
                this.warningLabel.Foreground = System.Windows.Media.Brushes.Green;
            }
        }
        private void KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                EnterPressed();
            }
            else if (e.Key == Key.Up)
            {
                this.thrustSlider.Value = this.thrustSlider.Value + 1;
            }
            else if (e.Key == Key.Down)
            {
                this.thrustSlider.Value = this.thrustSlider.Value - 1;
            }
            else if (e.Key == Key.Left)
            {
                moonLander.MoveHorizontally("left");
            }
            else if (e.Key == Key.Right)
            {
                moonLander.MoveHorizontally("right");
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void CrashGUILander()
        {
        }
        private void ResetGame()
        {
            Clock.Stop();
            moonLander.Clear();
            ResetGUILander();
            ResetSlider();
            ClearGraphs();
            ClearResultText();

        }
        private void ResetGUILander()
        {
            Thickness startPosition = new Thickness(0, 0, 0, 0);
            this.GUIlander.Margin = startPosition;

        }
        private void ClearGraphs()
        {
            this.heightVM.Points.Clear();
            this.heightPlot.InvalidatePlot();

            this.velocityVM.Points.Clear();
            this.velocityPlot.InvalidatePlot();
        }
        private void ClearResultText()
        {

            this.resultLabel.Content = "";
        }
        private void ResetSlider()
        {
            this.thrustSlider.Value = 0;
        }

        private void EnterPressed()
        {
            if (moonLander.HasLanded())
            {
                ResetGame();
            }
            else
            {
                Clock.Start();

            }

        }
    }

}
