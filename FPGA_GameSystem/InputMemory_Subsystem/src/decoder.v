/********************************************************************
* Module: decoder
* Author: Jamie O'Connor
* Date: 17-Nov-2025
* Description:
*   Pmod KYPD: 16-button Keypad decoder for Basys3 FPGA.
*   Implements row/column scanning with a debounce delay to provide a clean single key output per press.
*
* Inputs:
*   clk_100MHz  - 100 MHz system clock
*   row[3:0]    - Row inputs from keypad (active low)
*
* Outputs:
*   col[3:0]    - Column outputs to keypad (active low)
*   key_code[3:0] - 4-bit decoded key value (0-F)
*
* Parameters:
*   DEBOUNCE_DELAY - Number of clock cycles to wait for switch settling (~500 us at 100 MHz)
*
* Notes:
*   - Scans one column at a time, outputs key code when stable.
*   - Default key_code is 0 when no key is pressed.
*
********************************************************************/

`timescale 1ps/1ps

module decoder(
    input clk_100MHz,
    input [3:0] row,                // 4 keypad rows
    output reg [3:0] col,           // 4 column outputs
    output reg [3:0] key_code       // decoded key
    );

    parameter DEBOUNCE_DELAY = 50_000;              // settling time (50,000 cycles = 500 us @ 100MHz)

    reg [19:0] col_scan_timer = 0;
    reg [1:0] active_col = 0;    

    // Scan timer: advances one column after 100,000 cycles (~1ms)
    always @(posedge clk_100MHz) begin
        if (col_scan_timer == 99_999) begin
            col_scan_timer <= 0;
            active_col <= active_col + 1;
        end else
            col_scan_timer <= col_scan_timer + 1;
    end

    /* Column selection + key decoding
        - Activates one column at a time (active low)
        - After DEBOUNCE_DELAY cycles, reads row inputs to determine pressed key
    */
    always @(posedge clk_100MHz) begin

        case (active_col)
            2'b00: begin
                // Activate column 0
                col <= 4'b0111;
                if(col_scan_timer == DEBOUNCE_DELAY) begin
                    case (row)
                        4'b0111: key_code <= 4'b0001;
                        4'b1011: key_code <= 4'b0100;
                        4'b1101: key_code <= 4'b0111;
                        4'b1110: key_code <= 4'b0000;
                        default: key_code <= 4'b0000; // No-key code
                    endcase
                end
            end
            
            2'b01: begin
                // Activate column 1
                col <= 4'b1011;  
                if(col_scan_timer == DEBOUNCE_DELAY) begin
                    case (row)
                        4'b0111: key_code <= 4'b0010;
                        4'b1011: key_code <= 4'b0101;
                        4'b1101: key_code <= 4'b1000;
                        4'b1110: key_code <= 4'b1111;
                        default: key_code <= 4'b0000;
                    endcase
                end
            end

            2'b10: begin
                // Activate column 2
                col <= 4'b1101;
                if(col_scan_timer == DEBOUNCE_DELAY) begin
                    case (row)
                        4'b0111: key_code <= 4'b0011;
                        4'b1011: key_code <= 4'b0110;
                        4'b1101: key_code <= 4'b1001;
                        4'b1110: key_code <= 4'b1110;
                        default: key_code <= 4'b0000;
                    endcase
                end
            end

            2'b11: begin
                // Activate column 3
                col <= 4'b1110;
                if(col_scan_timer == DEBOUNCE_DELAY) begin
                    case (row)
                        4'b0111: key_code <= 4'b1010;
                        4'b1011: key_code <= 4'b1011;
                        4'b1101: key_code <= 4'b1100;
                        4'b1110: key_code <= 4'b1101;
                        default: key_code <= 4'b0000;
                    endcase
                end
            end
        endcase
    end

endmodule
