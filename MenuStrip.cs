using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

/// <summary>
/// UserControl called MenuStrip that contains a button to restart the game, exit the game, display the user's game statistics
/// (wins, losses, win/loss ration, and average time to complete a game), instructions button to show the user how to play and
/// what the other buttons do, and a button to display who created the game, when, and for what class.
/// Contains methods that setup the buttons with labels and colors, place them on the panel on the form, and handlers for when
/// the buttons are clicked. The public EventHandlers are referenced in MinesweeperForm.cs for when the game is restarted, exited,
/// or when the stat button is clicked because MinesweeperForm.cs has the functionality/information to accomplish them.
/// </summary>

namespace kneal3Minesweeper
{
    public partial class MenuStrip : UserControl
    {
        //Creates a new global panel for the menu strip.
        Panel backPanel = new Panel();

        //Public global handler for when the buttons are clicked.
        public EventHandler RestartButtonClicked;
        public EventHandler ExitButtonClicked;
        public EventHandler StatsButtonClicked;

        //Creates global buttons for the start, instructions, about, exit, and stats buttons.
        //Start and exit buttons are private so they can be accessed through MinesweeperForm.cs.
        public Button restartButton = new Button();
        public Button exitButton = new Button();
        public Button statsButton = new Button();

        Button instructionsButton = new Button();
        Button aboutButton = new Button();

        //Global variables to hold the size of the controls if we want to change the size of the gameboard in the future.
        int panelXSize = 240;
        int panelYSize = 50;

        int startXSize = 24;
        int startYSize = 24;

        int instructionsXSize = 24;
        int instructionsYSize = 24;

        int aboutXSize = 24;
        int aboutYSize = 24;

        int exitXSize = 72;
        int exitYSize = 24;

        int statsXSize = 72;
        int statsYSize = 24;

        /// <summary>
        /// Default setup for the menu strip. Calls method to add the controls to the UserControl.
        /// </summary>
        public MenuStrip()
        {
            InitializeComponent();

            //Call AddControlsToMenu to setup the controls.
            AddControlsToMenu();
        }//end MenuStrip.

        //Sets the size, locations, etc. of the controls for the menu strip.
        #region Setup Controls

        /// <summary>
        /// Set the locations of the controls on the panel.
        /// </summary>
        public void SetLocations()
        {
            //Places the start button in the center of the UserControl.
            restartButton.Location = new Point((panelXSize / 2) - (startXSize / 2), panelYSize / 4);

            //Places the instructions button on the top left size of the UserControl.
            instructionsButton.Location = new Point((panelXSize / 8) - (instructionsXSize / 8) - 2, 0);

            //Places the about button on the top right side of the UserControl.
            aboutButton.Location = new Point((panelXSize) - (panelXSize / 4) + 10, 0);

            //Places the exit button on the right side of the UserControl.
            exitButton.Location = new Point(panelXSize - exitXSize, panelYSize / 2);

            //Places the stats box on the bottom left side of the UserControl.
            statsButton.Location = new Point(0, panelYSize / 2);          
        }//end SetLocations.

        /// <summary>
        /// Sets the colors of the controls.
        /// </summary>
        public void SetColors()
        {
            //Sets the panel's color to gray.
            backPanel.BackColor = Color.Gray;

            //Sets the start button's color to yellow.
            restartButton.BackColor = Color.Yellow;

            //Sets the instruction button's color to white.
            instructionsButton.BackColor = Color.LightSkyBlue;

            //Sets the about button's color to green.
            aboutButton.BackColor = Color.Green;

            //Sets the exit button's color to red.
            exitButton.BackColor = Color.Red;
        }//end SetColors.

        /// <summary>
        /// Sets the sizes of the controls.
        /// </summary>
        public void SetSizes()
        {
            //Set the size of the controls using the global X and Y size variables.
            backPanel.Size = new Size(panelXSize, panelYSize);
            restartButton.Size = new Size(startXSize, startYSize);
            instructionsButton.Size = new Size(instructionsXSize, instructionsYSize);
            aboutButton.Size = new Size(aboutXSize, aboutYSize);
            exitButton.Size = new Size(exitXSize, exitYSize);
            statsButton.Size = new Size(statsXSize, statsYSize);
        }//end SetSizes.

        /// <summary>
        /// Sets the borders of the controls.
        /// </summary>
        public void SetBorders()
        {
            //Sets the border of the start button.
            restartButton.FlatStyle = FlatStyle.Flat;
            restartButton.FlatAppearance.BorderSize = 1;

            //Sets the border of the instructions button.
            instructionsButton.FlatStyle = FlatStyle.Flat;
            instructionsButton.FlatAppearance.BorderSize = 1;

            //Sets the border of the about button.
            aboutButton.FlatStyle = FlatStyle.Flat;
            aboutButton.FlatAppearance.BorderSize = 1;

            //Sets the border of the exit button.
            exitButton.FlatStyle = FlatStyle.Flat;
            exitButton.FlatAppearance.BorderSize = 1;

            //Sets the border of the stats button.
            statsButton.FlatStyle = FlatStyle.Flat;
            statsButton.FlatAppearance.BorderSize = 1;
        }//end SetBorders.

        /// <summary>
        /// Label the controls with an appropriate character or string by setting the text displayed on them.
        /// </summary>
        public void SetLabels()
        {
            //Label the start button with an R.
            restartButton.Text = "R";

            //Label the instructions button with a ?.
            instructionsButton.Text = "?";

            //Label the about button with an I.
            aboutButton.Text = "I";

            //Label the exit button with EXIT.
            exitButton.Text = "EXIT";

            //Label the stats button with STATS.
            statsButton.Text = "STATS";
        }//end SetupLabels.

        /// <summary>
        /// Method to add the correct button click handlers to the buttons.
        /// </summary>
        public void AddHandlers()
        {
            restartButton.Click += OnStartClickHandler;
            instructionsButton.Click += OnInstructionsClickHandler;
            aboutButton.Click += OnAboutClickHandler;
            exitButton.Click += OnExitClickHandler;
            statsButton.Click += OnStatsClickHandler;
        }//end AddHandlers.

        #endregion

        //Adds the controls to the menu strip.
        #region Add Controls

        /// <summary>
        /// Setup the menu by adding a panel, buttons, and labels to display the wins and losses.
        /// </summary>
        public void AddControlsToMenu()
        {
            //Calls methods to setup the properties of the controls.
            SetLocations();
            SetColors();
            SetSizes();
            SetBorders();
            SetLabels();
            AddHandlers();

            //Adds the panel, textbox, and buttons to the UserControl.
            this.Controls.Add(restartButton);
            this.Controls.Add(instructionsButton);
            this.Controls.Add(aboutButton);
            this.Controls.Add(exitButton);
            this.Controls.Add(statsButton);
            this.Controls.Add(backPanel);
        }//end AddControlsToMenu.

        #endregion

        //Button click handlers for when each button on the UserControl is clicked.
        #region Button Click Handlers

        /// <summary>
        /// Button click handler for when the start button is clicked and the game begins.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnStartClickHandler(object sender, EventArgs e)
        {
            //If the handler is not null, pass this to the handler. See MinesweeperForm.cs for restart button related methods.
            if(RestartButtonClicked != null)
            {
                this.RestartButtonClicked(this, e);
            }
        }//end OnStartClickHandler.

        /// <summary>
        /// Button click handler for when the instructions button is clicked. Displays message box with game instructions to the user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnInstructionsClickHandler(object sender, EventArgs e)
        {
            MessageBox.Show("                          INSTRUCTIONS                     \n" +
                            "-----------------------------------------------------------\n" +
                            "I: Display 'About' page for game\n" +
                            "?: Display this message\n" +
                            "EXIT: Exit the game\n" +
                            "STATS: Display statistics for current user gameplay\n" +
                            "R: Retart the game. Begins a timer. Object is to\n" +
                            "    identify all 10 mines on the board. Clicking on\n" +
                            "    a square will either display a bomb, a number,\n" +
                            "    or clear any empty squares near it. If it displays\n" +
                            "    a number, that means there are that many bombs in\n" +
                            "    the surrounding 8 squares. If you click a square\n" +
                            "    with a bomb, you lose. Have fun.");
        }//end OnInstructionsClickHandler.

        /// <summary>
        /// Button click handler for when the about button is clicked. Displays who coded the game, when and for what class in a message box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnAboutClickHandler(object sender, EventArgs e)
        {
            MessageBox.Show("MINESWEEPER GAME\n--------------------------\nCODED BY: Kobe Neal    \nCODED ON: 4/11 - 4/17/2021\nCLASS: CS3020 Section 001 ");
        }//end OnAboutClickHandler.

        /// <summary>
        /// Button click handler for when the exit button is clicked. Exits the program and displays a thank you message to the user in a message box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnExitClickHandler(object sender, EventArgs e)
        {
            //If the handler is not null, pass this to the button. See MinesweeperForm.cs for exit button related methods.
            if(ExitButtonClicked != null)
            {
                this.ExitButtonClicked(this, e);
            }
        }//end OnExitClickHandler.

        public void OnStatsClickHandler(object sender, EventArgs e)
        {
            if(StatsButtonClicked != null)
            {
                this.StatsButtonClicked(this, e);
            }
        }//end OnStatsClickHandler.

        #endregion
    }//end MenuStrip partial class.
}