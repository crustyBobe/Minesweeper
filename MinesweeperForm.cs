using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// Partial class for the Minesweeper Windows Form. Contains a static 2D array of Cell objects called grid and a static gridsize
/// to keep track of the size of the array. Creates an instance of MenuStrip, a control that contains buttons that allow the user
/// to reset the grid, display stats, exit the game, or display information or about.
/// Contains the logic for the Minesweeper game, with methods that place mines, check if the clicked cell is a mine, detects mines
/// in nearby cells, clears cells that contain whitespace, and the rest of the logic for the game.
/// Contains handlers that perform actions when cells are clicked, such as clearing the board, displaying message boxes, and checking
/// bordering cells for mines or whitespace.
/// Uses a StatusStrip placed on the GUI to keep track of the time the user has been playing. The time doesn't get reset to 0 unless
/// the program ends, instead it stops counting when the game is won, lost, or the form is reset and starts counting again when a cell
/// is clicked. The stats function uses the elapsed time to show the user their average game time.
/// Note: There is a 1/100 chance that the user will lose on their first click as the mines are placed before the first cell click, and
/// the stats will reset once the user ends the program so lifetime statistics are not tracked.
/// </summary>

namespace kneal3Minesweeper
{
    public partial class MinesweeperForm : Form
    {
        //Create a grid of cells on the form to hold the gameboard and set it's size to 10.
        static Cell[,] grid;
        static int gridSize = 10;

        //Create an instance of the MenuStrip.
        MenuStrip menuStrip = new MenuStrip();

        //Create a new global instance of a timer.
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        //Global variable to keep track of the time.
        int elapsedTime = 0;

        //Boolean to keep track of whether the game has started.
        bool gameHasStarted = false;

        //Global integers to keep track of the number of wins and losses.
        int wins = 0;
        int losses = 0;

        //Create a global instance of random for mine placement.
        static Random random = new Random();

        //Variable to track the number of cells that have been clicked to determine when the user has won the game.
        int clickedCells = 0;

        /// <summary>
        /// Default setup for the form.
        /// </summary>
        public MinesweeperForm()
        {
            //Default method to setup the form.
            InitializeComponent();

            //Call LoadMenuStrip to add the menu strip to the top of the form.
            LoadMenuStrip();

            //Call LoadGrid to setup the grid as an array of Cell objects.
            LoadGrid();

            //Call PlaceMines to add the mines to the board.
            PlaceMines();

            //Call DetectMines to label the cells adjacent to mines with the number of mines nearby.
            DetectMines();

            //Set the interval for the time and add the handler.
            timer.Interval = 1000;
            timer.Tick += OnTimerTick;

        }//end MinesweeperForm.

        //Loads the grid and the menustrip onto the form.
        #region Load / Reset Form Controls

        /// <summary>
        /// Method to load the grid array as an array of Cells, set their location on the form based on the Cell's CellSize property, then add them to the form.
        /// </summary>
        public void LoadGrid()
        {
            //Create a new array of cells with the global gridSize variable.
            grid = new Cell[gridSize, gridSize];

            //Netsed for loops to setup the grid and initialize it as an array of cells.
            for(int row = 0; row < grid.GetLength(0); row ++)
            {
                for(int col = 0; col < grid.GetLength(1); col ++)
                {
                    //Make the location on the grid an instance of a Cell.
                    grid[col, row] = new Cell();

                    //Get the location of the cell by calling the Row and Col properties.
                    grid[col, row].Row = row;
                    grid[col, row].Col = col;

                    //Add the OnCellClick button handler for when the cell has been clicked.
                    grid[col, row].CellHasBeenClicked += OnCellClick;

                    //Set the location of the grid using the CellSize property.
                    grid[col, row].Location = new Point(grid[col, row].CellSize * row, grid[col, row].CellSize * col + 50);
                    grid[col, row].Button.Enabled = true;

                    //Add the new Cell to the form.
                    this.Controls.Add(grid[col, row]);
                }
            }
        }//end LoadGrid.

        /// <summary>
        /// Load the menu strip to the form by adding the MenuStrip user control.
        /// </summary>
        public void LoadMenuStrip()
        {
            //Set the strip's location to the top left corner so it will display across the top of the game grid.
            menuStrip.Location = new Point(0, 0);

            //Add a handler for the start button on the menu strip for when it is clicked and the game can start.
            menuStrip.RestartButtonClicked += OnStartCellClick;

            //Add a handler for when the exit button on the menu strip is clicked.
            menuStrip.ExitButtonClicked += OnExitCellClick;

            menuStrip.StatsButtonClicked += OnStatsCellClick;

            //Add the strip to the form.
            this.Controls.Add(menuStrip);
        }

        /// <summary>
        /// Resets the game board when the user clicks the reset button
        /// </summary>
        private void ResetBoard()
        {
            //For loop to iterate through the grid and set the buttons to visible and enabled and set the text on the labels back to an empty string.
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i, j].Button.Enabled = true;
                    grid[i, j].Button.Visible = true;
                    grid[i, j].label.Text = "";
                }
            }

            //Reset clicked cells to 0.
            clickedCells = 0;

            //Call place mines and detect mines to re-place the mines on the board and detect them in the cells bordering them.
            PlaceMines();
            DetectMines();

            //Reset the timer.
            timer.Stop();

        }//end ResetBoard.

        #endregion

        //Places the mines on the grid and detects mines adjacent to cells.
        #region Mine Placement and Detection

        /// <summary>
        /// Method to place mines on the grid in random locations. Similar to the placement of battleships in the battleship class.
        /// </summary>
        public static void PlaceMines()
        {
            //For loop to iterate 10 times, once for each mine.
            for(int i = 0; i < 10; i ++)
            {
                //Boolean to check if the location is valid to place a mine and random x and y coordinates to place it at.
                bool isValid = false;
                int xLoc = random.Next(gridSize);
                int yLoc = random.Next(gridSize);

                //While the placement is not valid.
                while(!isValid)
                {
                    //If the button at the temporary location on the grid is visible, that means it hasn't been clicked and we can place a mine there.
                    //Also need to check if there's already a mine there so make sure the label's text isn't a *.
                    if(grid[xLoc, yLoc].label.Text != "*")
                    {
                        //Set is valid to true and write a * for a mine on the panel.
                        grid[xLoc, yLoc].label.Text = "*";
                        isValid = true;
                    }

                    //Otherwise find a new random location to place the mine and try again.
                    else
                    {
                        xLoc = random.Next(gridSize);
                        yLoc = random.Next(gridSize);
                    }
                }//end while.
            }//end for.
        }//end PlaceMines.

        /// <summary>
        /// Checks cells adjacent to a cell to determine if there are any mines, and dipslays the appropriate number based on the number of mines.
        /// </summary>
        public void DetectMines()
        {
            //Keep track of the number of mines nearby.
            int count = 0;

            //Temporary instance of a cell.
            Cell temp = new Cell();

            //Nested forloops to check every
            for(int row = 0; row < grid.GetLength(0); row ++)
            {
                for(int col = 0; col < grid.GetLength(1); col ++)
                {
                    //Set the temporary cell to the current location on the grid.
                    temp = grid[row, col];

                    //Call the 8 methods to check the cells surrounding the cell and see if there is a mine.
                    //The methods return a 1 if there is, so the number of surrounding mines is the sum of the methods.
                    count = CheckDiagDownLeft(temp) + CheckDiagDownRight(temp) + CheckDiagUpLeft(temp) + CheckDiagUpRight(temp) +
                            CheckMineDown(temp) + CheckMineUp(temp) + CheckMineLeft(temp) + CheckMineRight(temp);

                    //Call the LabelCell method to label the cell with the appropriate number, empty string, or mine based on the methods and the count.
                    LabelCell(temp, count);
                }
            }
        }//end DetectMines.

        /// <summary>
        /// Method to label the cell based on the count. If CheckMine returns true, label it as a * because it is a mine.
        /// If the count is 0 and CheckMine is false, label it as an empty string because there are no surrounding mines.
        /// Otherwise write the count to the label for the amount of surrounding mines.
        /// </summary>
        /// <param name="temp"></param>
        /// <param name="count"></param>
        private void LabelCell(Cell temp, int count)
        {
            //If the count is 0 and the cell isn't a mine, set the text to an empty string.
            if (count == 0 && !CheckIfMine(temp))
            {
                temp.label.Text.Equals("");
            }

            //If the cell is a mine, set the text to a *.
            else if (CheckIfMine(temp))
            {
                temp.label.Text.Equals("*");
            }

            //Otherwise set the text to the count for the number of mines nearby.
            else
            {
                temp.label.Text = Convert.ToString(count);
            }
        }//end LabelCell.

        /// <summary>
        /// Checks if the current cell is a mine. Returns true if it is.
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        private bool CheckIfMine(Cell temp)
        {
            //If the text on the label is a *, return true because it's a mine.
            if(temp.label.Text.Equals("*"))
            {
                return true;
            }

            return false;
        }

        #endregion

        //Check adjacent cells to a cell to see if they have a mine and return 1 if they do.
        #region Check Mine For Cell Numbering Methods

        private int CheckDiagUpLeft(Cell temp)
        {
            if(temp.Col - 1 >= 0 && temp.Row - 1 >= 0 && !CheckIfMine(temp))
            {
                if(grid[temp.Col - 1, temp.Row - 1].label.Text.Equals("*"))
                {
                    return 1;
                }
            }

            return 0;
        }//end CheckDiagUpLeft
        
        private int CheckDiagUpRight(Cell temp)
        {
            if(temp.Col - 1 >= 0 && temp.Row + 1 < grid.GetLength(0) && !CheckIfMine(temp))
            {
                if(grid[temp.Col - 1, temp.Row + 1].label.Text.Equals("*"))
                {
                    return 1;
                }
            }

            return 0;
        }//end CheckDiagUpRight.

        private int CheckDiagDownLeft(Cell temp)
        {
            if(temp.Col + 1 < grid.GetLength(0) && temp.Row - 1 >= 0 && !CheckIfMine(temp))
            {
                if(grid[temp.Col + 1, temp.Row - 1].label.Text.Equals("*"))
                {
                    return 1;
                }
            }

            return 0;
        }//end CheckDiagDownLeft

        private int CheckDiagDownRight(Cell temp)
        {
            if(temp.Col + 1 < grid.GetLength(0) && temp.Row + 1 < grid.GetLength(0) && !CheckIfMine(temp))
            {
                if(grid[temp.Col + 1, temp.Row + 1].label.Text.Equals("*"))
                {
                    return 1;
                }
            }

            return 0;
        }//end CheckDiagDownRight.

        private int CheckMineUp(Cell temp)
        {
            if(temp.Col - 1 >= 0 && !CheckIfMine(temp))
            {
                if(grid[temp.Col - 1, temp.Row].label.Text.Equals("*"))
                {
                    return 1;
                }
            }

            return 0;
        }//end CheckMineUp,

        private int CheckMineDown(Cell temp)
        {
            if(temp.Col + 1 < grid.GetLength(0) && !CheckIfMine(temp))
            {
                if(grid[temp.Col + 1, temp.Row].label.Text.Equals("*"))
                {
                    return 1;
                }
            }

            return 0;
        }//end CheckMineDown,

        private int CheckMineLeft(Cell temp)
        {
            if(temp.Row - 1 >= 0 && !CheckIfMine(temp))
            {
                if(grid[temp.Col, temp.Row - 1].label.Text.Equals("*"))
                {
                    return 1;
                }
            }

            return 0;
        }//end CheckMineLeft.

        private int CheckMineRight(Cell temp)
        {
            if(temp.Row + 1 < grid.GetLength(0) && !CheckIfMine(temp))
            {
                if(grid[temp.Col, temp.Row + 1].label.Text.Equals("*"))
                {
                    return 1;
                }
            }

            return 0;
        }//end CheckMineRight.

        #endregion

        //Handler for when the cell is clicked to clear any adjacent empty cells.
        #region Handlers

        /// <summary>
        /// Handler to clear the empty cells near a cell when it is clicked if it's empty.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnCellClick(object sender, EventArgs e)
        {
            //Cast the temporary sender to a Cell.
            Cell temp = (Cell)sender;

            if(!gameHasStarted)
            {
                timer.Start();
                gameHasStarted = true;
            }

            clickedCells++;
            //Check if the game was won on the cell click.
            CheckIfWon();

            //Check if the clicked cell is a mine.
            CheckClickedMine(temp);

            //Call check methods to check all adjacent cells to see if they're blank.
            CheckUp(temp);
            CheckDown(temp);
            CheckLeft(temp);
            CheckRight(temp);
        }//end OnCellClick.

        /// <summary>
        /// Handler for when the start button is clicked and the user wants to start another game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnStartCellClick(object sender, EventArgs e)
        {
            ResetBoard();
        }//end OnStartCellClick.

        /// <summary>
        /// Handler for when the exit button on the menu strip is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnExitCellClick(object sender, EventArgs e)
        {
            //Display a message to the user telling them they have chosen to exit.
            MessageBox.Show("You have chosen to exit.\nThank you for playing.");

            //Wait 100ms and close the form to exit the program.
            Thread.Sleep(250);
            this.Close();
        }//end OnExitCellClick.

        /// <summary>
        /// Handler for when the about button is clicked. Displays the user's stasts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnStatsCellClick(object sender, EventArgs e)
        {
            //Get the total number of games played by adding the wins and the losses.
            int total = wins + losses;

            //Check if the total is 0 to avoid a divide by 0 error. Tried a try/catch block but it didn't work.
            if(total == 0)
            {
                MessageBox.Show("You must complete at least one game before checking your stats.");
            }

            //Display the number of wins, losses, win/loss ratio, and the average time. Stats will reset when the game is ended.
            else
            {
                MessageBox.Show($"Wins: {wins}\nLoses: {losses}\nWin Loss Ratio: {wins}:{losses}\nAverage Time: {elapsedTime / total} sec.");
            }   
        }//end OnStatsCellClick.

        /// <summary>
        /// Handler for when the timer ticks to display the time and keep track of the elapsed time.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnTimerTick(object sender, EventArgs e)
        {
            if(gameHasStarted)
            {
                elapsedTime++;
                statusStripTimerLabel.Text = $"Timer: {elapsedTime}";
            }
        }//end OnTimerTick.

        #endregion

        //Checks adjacent cells to see if they are empty and they can be cleared.
        #region Check Cells to Clear On Cell Click Methods

        /// <summary>
        /// Checks the cells above to see if they are clear and can be clicked.
        /// </summary>
        /// <param name="temp"></param>
        private void CheckUp(Cell temp)
        {
            if(temp.Col - 1 >= 0)
            {
                //If the cell is empty, i.e. it's label shows nothing.
                if(grid[temp.Col - 1, temp.Row].label.Text.Equals(""))
                {
                    //Call PerformClick() to click the empty cell to show the empty panel and label beneath.
                    grid[temp.Col - 1, temp.Row].Button.PerformClick();
                }
            }
        }//end CheckUp.

        /// <summary>
        /// Checks the cells below to see if they are clear and can be clicked.
        /// </summary>
        /// <param name="temp"></param>
        private void CheckDown(Cell temp)
        {
            if(temp.Col + 1 < grid.GetLength(0))
            {
                //If the cell is empty.
                if(grid[temp.Col + 1, temp.Row].label.Text.Equals(""))
                {
                    //Call PerformClick() to click the cell and clear it.
                    grid[temp.Col + 1, temp.Row].Button.PerformClick();
                }
            }
        }//end CheckDown.

        /// <summary>
        /// Checks the cells to the left to see if they can be clicked and cleared.
        /// </summary>
        /// <param name="temp"></param>
        private void CheckLeft(Cell temp)
        {
            if(temp.Row - 1 >= 0)
            {
                //If the cell is empty.
                if(grid[temp.Col, temp.Row - 1].label.Text.Equals(""))
                {
                    //Click the button on the cell to clear it.
                    grid[temp.Col, temp.Row - 1].Button.PerformClick();
                }
            }
        }//end CheckLeft.

        /// <summary>
        /// Checks the cells to the right to see if they can be clicked and cleared.
        /// </summary>
        /// <param name="temp"></param>
        private void CheckRight(Cell temp)
        {
            if(temp.Row + 1 < grid.GetLength(0))
            {
                //If the cell is empty.
                if(grid[temp.Col, temp.Row + 1].label.Text.Equals(""))
                {
                    //Click the button on the cell to clear it.
                    grid[temp.Col, temp.Row + 1].Button.PerformClick();
                }
            }
        }//end CheckRight.

        #endregion

        #region Check Win / Lose

        /// <summary>
        /// Checks if the cell that was clicked is a mine. If it is, display a message that the play has lost and removes the buttons.
        /// </summary>
        /// <param name="temp"></param>
        private void CheckClickedMine(Cell temp)
        {
            //If the passed mine was checked, display a message to the user telling them they lost and clear out the board.
            if (CheckIfMine(temp))
            {
                //Set gameHasStarted to false so to stop the timer so it can be started again later.
                gameHasStarted = false;

                //Display a message to the user saying they lost.
                MessageBox.Show("YOU LOSE");

                //Increment the number of times the user has lost.
                losses++;

                //Loop through the grid and disable each cell so the user can't continue playing.
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    for (int j = 0; j < grid.GetLength(1); j++)
                    {
                        //Grey out the cells on the grid.
                        grid[i, j].Button.Visible = false;
                    }
                }
            }
        }//end CheckClickedMine.

        /// <summary>
        /// Method to check if the user has won the game by checking each cell and seeing if the buttons over the mines are still visible.
        /// </summary>
        public void CheckIfWon()
        {
            //If there are 10 mines that are still covered.
            if(clickedCells == 90)
            {
                //Set gameHasStarted to false so to stop the timer so it can be started again later.
                gameHasStarted = false;

                //Display a message to the user saying they've won the game and ask them to play again.
                MessageBox.Show("Congratulations! You won!\nClick the R to reset the board and play again");

                //Increment the total number of wins.
                wins++;

                //Loop through the grid and disable each cell so the user can't continue playing.
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    for (int j = 0; j < grid.GetLength(1); j++)
                    {
                        //Grey out the cells on the grid.
                        grid[i, j].Button.Visible = false;
                    }
               }
            }
        }//end CheckIfWon.

        #endregion

    }//end MinesweeperForm partial class.
}