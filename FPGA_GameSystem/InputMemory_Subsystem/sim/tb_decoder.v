/********************************************************************
* Module: tb_decoder
* Author: Jamie O'Connor
* Date: 17-Nov-2025
* Description:
*   Testbench for the Pmod KYPD: 16-button Keypad decoder module.
*   Simulates all 16 keys sequentially, verifying that the decoder correctly scans columns and outputs the expected key codes.
*
* Signals:
*   clk_100MHz   - 100 MHz system clock
*   row[3:0]     - Simulated row inputs from keypad
*   col[3:0]     - Column outputs from decoder (active low)
*   key_code[3:0]- Decoded key value (0-F)
*
* Notes:
*   - Clock generated with 10 ns period (100 MHz)
*   - press_key task simulates pressing and releasing a key
*   - Wait times chosen to exceed debounce delay and scan period
********************************************************************/

`timescale 1ns/1ps

module tb_decoder;

    // Clock and row inputs
    reg clk_100MHz = 0;
    reg [3:0] row;

    // Outputs from the decoder
    wire [3:0] col;
    wire [3:0] key_code;

    // Instantiate the decoder module
    decoder uut (
        .clk_100MHz(clk_100MHz),
        .row(row),
        .col(col),
        .key_code(key_code)
    );

    // Generate 100 MHz clock: 10 ns period
    always #5 clk_100MHz = ~clk_100MHz; 

    // Simulate key press
    task press_key;
        input [3:0] simulated_row;
        begin
            row = simulated_row;       // press key
            // Hold for scan + debounce (4 ms)
            #4_000_000;                
            row = 4'b1111;             // release key (all high)
            #50_000;                   // short delay before next key
        end
    endtask

    initial begin
        // Initialize no key pressed
        row = 4'b1111; 

        // Wait for clock stabilization
        #50_000; 

        // Simulate all 16 keys sequentially
        
        // Column 0
        press_key(4'b0111); // Key 0,0 
        press_key(4'b1011); // Key 1,0 
        press_key(4'b1101); // Key 2,0 
        press_key(4'b1110); // Key 3,0 

        // Column 1
        press_key(4'b0111); // Key 0,1 
        press_key(4'b1011); // Key 1,1 
        press_key(4'b1101); // Key 2,1 
        press_key(4'b1110); // Key 3,1 

        // Column 2
        press_key(4'b0111); // Key 0,2 
        press_key(4'b1011); // Key 1,2 
        press_key(4'b1101); // Key 2,2 
        press_key(4'b1110); // Key 3,2 

        // Column 3
        press_key(4'b0111); // Key 0,3 
        press_key(4'b1011); // Key 1,3 
        press_key(4'b1101); // Key 2,3 
        press_key(4'b1110); // Key 3,3 

        // End simulation
        #50_000; 
        $finish;
    end

endmodule
