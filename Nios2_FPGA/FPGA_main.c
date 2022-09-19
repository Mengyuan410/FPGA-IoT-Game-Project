/*
 * "Small Hello World" example.
 *
 * This example prints 'Hello from Nios II' to the STDOUT stream. It runs on
 * the Nios II 'standard', 'full_featured', 'fast', and 'low_cost' example
 * designs. It requires a STDOUT  device in your system's hardware.
 *
 * The purpose of this example is to demonstrate the smallest possible Hello
 * World application, using the Nios II HAL library.  The memory footprint
 * of this hosted application is ~332 bytes by default using the standard
 * reference design.  For a more fully featured Hello World application
 * example, see the example titled "Hello World".
 *
 * The memory footprint of this example has been reduced by making the
 * following changes to the normal "Hello World" example.
 * Check in the Nios II Software Developers Manual for a more complete
 * description.
 *
 * In the SW Application project (small_hello_world):
 *
 *  - In the C/C++ Build page
 *
 *    - Set the Optimization Level to -Os
 *
 * In System Library project (small_hello_world_syslib):
 *  - In the C/C++ Build page
 *
 *    - Set the Optimization Level to -Os
 *
 *    - Define the preprocessor option ALT_NO_INSTRUCTION_EMULATION
 *      This removes software exception handling, which means that you cannot
 *      run code compiled for Nios II cpu with a hardware multiplier on a core
 *      without a the multiply unit. Check the Nios II Software Developers
 *      Manual for more details.
 *
 *  - In the System Library page:
 *    - Set Periodic system timer and Timestamp timer to none
 *      This prevents the automatic inclusion of the timer driver.
 *
 *    - Set Max file descriptors to 4
 *      This reduces the size of the file handle pool.
 *
 *    - Check Main function does not exit
 *    - Uncheck Clean exit (flush buffers)
 *      This removes the unneeded call to exit when main returns, since it
 *      won't.
 *
 *    - Check Don't use C++
 *      This builds without the C++ support code.
 *
 *    - Check Small C library
 *      This uses a reduced functionality C library, which lacks
 *      support for buffering, file IO, floating point and getch(), etc.
 *      Check the Nios II Software Developers Manual for a complete list.
 *
 *    - Check Reduced device drivers
 *      This uses reduced functionality drivers if they're available. For the
 *      standard design this means you get polled UART and JTAG UART drivers,
 *      no support for the LCD driver and you lose the ability to program
 *      CFI compliant flash devices.
 *
 *    - Check Access device drivers directly
 *      This bypasses the device file system to access device drivers directly.
 *      This eliminates the space required for the device file system services.
 *      It also provides a HAL version of libc services that access the drivers
 *      directly, further reducing space. Only a limited number of libc
 *      functions are available in this configuration.
 *
 *    - Use ALT versions of stdio routines:
 *
 *           Function                  Description
 *        ===============  =====================================
 *        alt_printf       Only supports %s, %x, and %c ( < 1 Kbyte)
 *        alt_putstr       Smaller overhead than puts with direct drivers
 *                         Note this function doesn't add a newline.
 *        alt_putchar      Smaller overhead than putchar with direct drivers
 *        alt_getchar      Smaller overhead than getchar with direct drivers
 *
 */
/*
 * "Small Hello World" example.
 *
 * This example prints 'Hello from Nios II' to the STDOUT stream. It runs on
 * the Nios II 'standard', 'full_featured', 'fast', and 'low_cost' example
 * designs. It requires a STDOUT  device in your system's hardware.
 *
 * The purpose of this example is to demonstrate the smallest possible Hello
 * World application, using the Nios II HAL library.  The memory footprint
 * of this hosted application is ~332 bytes by default using the standard
 * reference design.  For a more fully featured Hello World application
 * example, see the example titled "Hello World".
 *
 * The memory footprint of this example has been reduced by making the
 * following changes to the normal "Hello World" example.
 * Check in the Nios II Software Developers Manual for a more complete
 * description.
 *
 * In the SW Application project (small_hello_world):
 *
 *  - In the C/C++ Build page
 *
 *    - Set the Optimization Level to -Os
 *
 * In System Library project (small_hello_world_syslib):
 *  - In the C/C++ Build page
 *
 *    - Set the Optimization Level to -Os
 *
 *    - Define the preprocessor option ALT_NO_INSTRUCTION_EMULATION
 *      This removes software exception handling, which means that you cannot
 *      run code compiled for Nios II cpu with a hardware multiplier on a core
 *      without a the multiply unit. Check the Nios II Software Developers
 *      Manual for more details.
 *
 *  - In the System Library page:
 *    - Set Periodic system timer and Timestamp timer to none
 *      This prevents the automatic inclusion of the timer driver.
 *
 *    - Set Max file descriptors to 4
 *      This reduces the size of the file handle pool.
 *
 *    - Check Main function does not exit
 *    - Uncheck Clean exit (flush buffers)
 *      This removes the unneeded call to exit when main returns, since it
 *      won't.
 *
 *    - Check Don't use C++
 *      This builds without the C++ support code.
 *
 *    - Check Small C library
 *      This uses a reduced functionality C library, which lacks
 *      support for buffering, file IO, floating point and getch(), etc.
 *      Check the Nios II Software Developers Manual for a complete list.
 *
 *    - Check Reduced device drivers
 *      This uses reduced functionality drivers if they're available. For the
 *      standard design this means you get polled UART and JTAG UART drivers,
 *      no support for the LCD driver and you lose the ability to program
 *      CFI compliant flash devices.
 *
 *    - Check Access device drivers directly
 *      This bypasses the device file system to access device drivers directly.
 *      This eliminates the space required for the device file system services.
 *      It also provides a HAL version of libc services that access the drivers
 *      directly, further reducing space. Only a limited number of libc
 *      functions are available in this configuration.
 *
 *    - Use ALT versions of stdio routines:
 *
 *           Function                  Description
 *        ===============  =====================================
 *        alt_printf       Only supports %s, %x, and %c ( < 1 Kbyte)
 *        alt_putstr       Smaller overhead than puts with direct drivers
 *                         Note this function doesn't add a newline.
 *        alt_putchar      Smaller overhead than putchar with direct drivers
 *        alt_getchar      Smaller overhead than getchar with direct drivers
 *
 */
#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include "system.h"
#include "altera_up_avalon_accelerometer_spi.h"
#include "altera_avalon_timer_regs.h"
#include "altera_avalon_timer.h"
#include "altera_avalon_pio_regs.h"
#include "sys/alt_irq.h"
#include <stdlib.h>
#include "alt_types.h"
#include "sys/times.h"
#include <unistd.h>
#include <sys/alt_stdio.h>
#include <math.h>
#define OFFSET -32
#define PWM_PERIOD 16
#define CHARLIM 256		// Maximum character length of what the user places in memory.  Increase to allow longer sequences
#define QUITLETTER '~' 		// Letter to kill all processing


alt_8 pwm = 0;
alt_u8 led;
alt_8 cd_countdown = 0;
int init_flag = 0;
int bomb_flag = 0;
int level;
int TAPSx = 31;
int TAPSy = 31;


void led_write(alt_u8 led_pattern) {
    IOWR(LED_BASE, 0, led_pattern);
}

//void convert_read(alt_32 acc_read, int * level, alt_u8 * led) {
//    acc_read += OFFSET;
//    alt_u8 val = (acc_read >> 6) & 0x07;
//    * led = (8 >> val) | (8 << (8 - val));
//    * level = (acc_read >> 1) & 0x1f;
//}

void sys_timer_isr() {
    IOWR_ALTERA_AVALON_TIMER_STATUS(TIMER_BASE, 0);
    cd_countdown = led_cooling_down(cd_countdown);
}

void timer_init(void * isr) {
    IOWR_ALTERA_AVALON_TIMER_CONTROL(TIMER_BASE, 0x0003);
    IOWR_ALTERA_AVALON_TIMER_STATUS(TIMER_BASE, 0);
    IOWR_ALTERA_AVALON_TIMER_PERIODL(TIMER_BASE, 0x7840);
    IOWR_ALTERA_AVALON_TIMER_PERIODH(TIMER_BASE, 0x017D);
    alt_irq_register(TIMER_IRQ, 0, isr);
    IOWR_ALTERA_AVALON_TIMER_CONTROL(TIMER_BASE, 0x0007);
}

void sys_timer_isr_2() {
    IOWR_ALTERA_AVALON_TIMER_STATUS(TIMER_SEND_BASE, 0);
    collect_game_data();
}

void timer_init_2(void * isr) {
    IOWR_ALTERA_AVALON_TIMER_CONTROL(TIMER_SEND_BASE, 0x0003);
    IOWR_ALTERA_AVALON_TIMER_STATUS(TIMER_SEND_BASE, 0);
    IOWR_ALTERA_AVALON_TIMER_PERIODL(TIMER_SEND_BASE, 0x5000);
    IOWR_ALTERA_AVALON_TIMER_PERIODH(TIMER_SEND_BASE, 0x0000);
    alt_irq_register(TIMER_SEND_IRQ, 0, isr);
    IOWR_ALTERA_AVALON_TIMER_CONTROL(TIMER_SEND_BASE, 0x0007);
}

int led_cooling_down(i) {
	if (i==0){
			IOWR_ALTERA_AVALON_PIO_DATA(LED_BASE, 0b0000000000);
		}
	if (i!=0){
		if (i==10){
			IOWR_ALTERA_AVALON_PIO_DATA(LED_BASE, 0b1111111111);
		}
		if (i==9){
			IOWR_ALTERA_AVALON_PIO_DATA(LED_BASE, 0b0111111111);
		}
		if (i==8){
			IOWR_ALTERA_AVALON_PIO_DATA(LED_BASE, 0b0011111111);
		}
		if (i==7){
			IOWR_ALTERA_AVALON_PIO_DATA(LED_BASE, 0b0001111111);
		}
		if (i==6){
			IOWR_ALTERA_AVALON_PIO_DATA(LED_BASE, 0b0000111111);
		}
		if (i==5){
			IOWR_ALTERA_AVALON_PIO_DATA(LED_BASE, 0b0000011111);
		}
		if (i==4){
			IOWR_ALTERA_AVALON_PIO_DATA(LED_BASE, 0b0000001111);
		}
		if (i==3){
			IOWR_ALTERA_AVALON_PIO_DATA(LED_BASE, 0b0000000111);
		}
		if (i==2){
			IOWR_ALTERA_AVALON_PIO_DATA(LED_BASE, 0b0000000011);
		}
		if (i==1){
			IOWR_ALTERA_AVALON_PIO_DATA(LED_BASE, 0b0000000001);
		}
		i--;
	}
	return i;
}


void switch_data_in_to_int(int *switch_data){
	switch(*switch_data){
	case 1:
        *switch_data =  0;
        break;
	case 2:
        *switch_data =  1;
        break;
	case 4:
        *switch_data =  2;
        break;
	case 8:
        *switch_data =  3;
        break;
	case 16:
        *switch_data =  4;
        break;
	case 32:
        *switch_data =  5;
        break;
	case 64:
        *switch_data =  6;
        break;
	case 128:
        *switch_data =  7;
        break;
	case 256:
        *switch_data =  8;
        break;
	case 512:
        *switch_data =  9;
        break;
	default:
		*switch_data =  0;
		break;
	}
}

//Gets the binary representation of the character
int getBin(char letter){
	/*Based on the character entered, we convert to binary so the 7-segment knows which lights to turn on.
	The 7-segment has inverted logic so a 0 means the light is on and a 1 means the light is off.
	The rightmost bit starts the index at HEX#[0], and the leftmost bit is HEX#[6], the pattern
	for the 7-segment is shown in the DE0_C5 User Manual*/
	switch(letter){
	case '0':
		return 0b1000000;
	case '1':
		return 0b1111001;
	case '2':
		return 0b0100100;
	case '3':
		return 0b0110000;
	case '4':
		return 0b0011001;
	case '5':
		return 0b0010010;
	case '6':
		return 0b0000010;
	case '7':
		return 0b1111000;
	case '8':
		return 0b0000000;
	case '9':
		return 0b0010000;
	case 'A':
		return 0b0001000;
	case 'B'://Lowercase
		return 0b0000011;
	case 'C':
		return 0b1000110;
	case 'D'://Lowercase
		return 0b0100001;
	case 'E':
		return 0b0000110;
	case 'F':
		return 0b0001110;
	case 'G':
		return 0b0010000;
	case 'H':
		return 0b0001001;
	case 'I':
		return 0b1111001;
	case 'J':
		return 0b1110001;
	case 'K':
		return 0b0001010;
	case 'L':
		return 0b1000111;
	case 'N':
		return 0b0101011;
	case 'O':
		return 0b1000000;
	case 'P':
		return 0b0001100;
	case 'Q':
		return 0b0011000;
	case 'R'://Lowercase
		return 0b0101111;
	case 'S':
		return 0b0010010;
	case 'T':
		return 0b0000111;
	case 'U':
		return 0b1000001;
	case 'V':
		return 0b1100011;
	case 'X':
		return 0b0011011;
	case 'Y':
		return 0b0010001;
	case 'Z':
		return 0b0100100;
	default:
		return 0b1111111;
	}
}

//Prints each of the letters out to the screen
void print(int let5, int let4, int let3, int let2, int let1, int let0){
	//Takes the binary value for each letter and places it on each of the six 7-segment displays
	IOWR_ALTERA_AVALON_PIO_DATA(HEX5_BASE, let5);
	IOWR_ALTERA_AVALON_PIO_DATA(HEX4_BASE, let4);
	IOWR_ALTERA_AVALON_PIO_DATA(HEX3_BASE, let3);
	IOWR_ALTERA_AVALON_PIO_DATA(HEX2_BASE, let2);
	IOWR_ALTERA_AVALON_PIO_DATA(HEX1_BASE, let1);
	IOWR_ALTERA_AVALON_PIO_DATA(HEX0_BASE, let0);
	return;
}


char generate_text(char curr, int *length, char *text, int *running) {
	if(curr == '\n') return curr;								// If the line is empty, return nothing.
	int idx = 0;										// Keep track of how many characters have been sent down for later printing
	char newCurr = curr;

	while (newCurr != EOF && newCurr != '\n'){						// Keep reading characters until we get to the end of the line
		if (newCurr == QUITLETTER) { *running = 0; }					// If quitting letter is encountered, setting running to 0
		text[idx] = newCurr;								// Add the next letter to the text buffer
		idx++;										// Keep track of the number of characters read
		newCurr = alt_getchar();							// Get the next character
	}
	*length = idx;

	return newCurr;
}
void display_message(char *text){
	char *last = strtok(text, ",");
	// char *last = strrchr(text, ',');
	int hexdisplay[6] = {getBin(last[3]), getBin(last[2]), 0b1111111, 0b1111111, getBin(last[1]), getBin(last[0])};

	print(hexdisplay[5], hexdisplay[4], hexdisplay[3], hexdisplay[2], hexdisplay[1], hexdisplay[0]);

}

void collect_username(){
    int usernameout = 0;
    int hexdisplay[6] = {0b1111111,0b1111111,0b1111111,0b1111111, 0b1111111, 0b1111111};
	int digit;
	int switch_datain = 0b0000000000;
	int tmp = 0b0000000000;
    int index = 0;
    int switch_data_single = 0;
	char charValue;

    while(index<4){
        switch_datain = IORD_ALTERA_AVALON_PIO_DATA(SWITCH_BASE);
        switch_data_single = tmp ^ switch_datain;
		if (switch_data_single != 0){ // if switch_datain has changed
			tmp = switch_datain;
			switch_data_in_to_int(&switch_data_single);
			charValue=switch_data_single+'0'; // int to char
            digit = getBin(charValue);
            usernameout += switch_data_single*pow(10,(3-index));

			int temp;
			for (int i=2; i>=0; i--){
				temp = hexdisplay[i];
				hexdisplay[i+1] = temp;
			}
			hexdisplay[0] = digit;
			printf("%c",charValue);
			print(hexdisplay[5], hexdisplay[4], hexdisplay[3], hexdisplay[2], hexdisplay[1], hexdisplay[0]);
            index++;
        }
    }
	int button_datain;
	button_datain = IORD_ALTERA_AVALON_PIO_DATA(BUTTON_BASE);
	while (button_datain != 2){
		button_datain = IORD_ALTERA_AVALON_PIO_DATA(BUTTON_BASE);
		if (button_datain == 2){
			alt_printf("Usernamex%x\n",usernameout);
			print(0b1111111, 0b1111111, 0b1111111, 0b1111111, 0b1111111, 0b1111111);
		}
	}


}


void collect_game_data() {
	alt_32 x_read, y_read, z_read;
	int button_datain;
	alt_up_accelerometer_spi_dev * acc_dev;
	acc_dev = alt_up_accelerometer_spi_open_dev("/dev/accelerometer_spi");
	if (acc_dev == NULL) { // if return 1, check if the spi ip name is "accelerometer_spi"
		printf("error1");
	}

    alt_up_accelerometer_spi_read_x_axis(acc_dev, & x_read);
    alt_up_accelerometer_spi_read_y_axis(acc_dev, & y_read);
    alt_up_accelerometer_spi_read_z_axis(acc_dev, & z_read);

    alt_printf(" Rx%x/",x_read);
    alt_printf(" Ry%x/",y_read);
    alt_printf(" Rz%x/",z_read);

    button_datain = IORD_ALTERA_AVALON_PIO_DATA(BUTTON_BASE);
    alt_printf("B%x/\n",button_datain);//button_datain=0/1bomb/2eat

    if (button_datain == 1 && bomb_flag == 0 && cd_countdown == 0){
        bomb_flag = 1;
        cd_countdown = 10;
    }
    if (button_datain !=1){
        bomb_flag = 0;
    }

    if (button_datain == 2 && init_flag == 0){
        init_flag = 1;
        alt_printf("Ix%x/ Iy%x/ Iz%x\/n",x_read,y_read,z_read);
    }
    if (button_datain !=2){
        init_flag = 0;
    }
}

int read_chars() {
	char text[2*CHARLIM];
	char prevLetter = '!';
	int length = 0;
	int running = 1;

	prevLetter = alt_getchar();							// Extract the first character (and create a hold until one arrives)
	prevLetter = generate_text(prevLetter, &length, text, &running);		// Process input text

	if(text[0] == 'P'){
	    collect_username();
	}
	timer_init(sys_timer_isr);
	timer_init_2(sys_timer_isr_2);
	while (1) {									// Keep running until QUITLETTER is encountered
		//printf("open");
		//print(0b1111111, 0b1100011, 0b1111111, 0b1111111, 0b1111111, 0b1111111);
		prevLetter = alt_getchar();							// Extract the first character (and create a hold until one arrives)
		prevLetter = generate_text(prevLetter, &length, text, &running);		// Process input text
		//print(0b1111111, 0b1100011, 0b1111111, 0b1111111, 0b1111111, 0b1111111);
		if(text[0] == ',' ) {
			display_message(text);
		}
	}
	return 0;
}

int main() {
	return read_chars();
}
