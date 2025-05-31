# Computer Architecture Simulator

A Windows Forms application written in C# that simulates the **Mano Microprogrammed Computer**, inspired by the architecture described in M. Morris Mano's foundational books on computer system design.

## 📚 Overview

This project serves as a simulation of a basic microprogrammed computer. It is intended for educational purposes, helping students visualize and interact with low-level operations such as:

- Register transfers
- ALU operations
- Control signal flows
- Microinstruction sequencing

The simulator allows users to step through the operation of a simplified CPU and observe how each control signal affects data movement and processing.

## 🧩 Features

- GUI-based simulation of the microprogram control unit
- Emulation of CPU registers, memory, and data paths
- Step-by-step microinstruction execution
- Real-time visual feedback of control signals and state changes
- Load and run custom instruction sequences

## 🛠 Technologies Used

- **Language:** C#
- **Platform:** .NET Framework (Windows Forms)
- **Development Environment:** Visual Studio

## 🗂 Project Structure

─ Control.cs # Contains microprogram control logic and signal definitions
─ Cpu.cs # Models the CPU, its registers, and micro-operations
─ Form1.cs # Main application logic for the GUI
─ Form1.Designer.cs # Windows Forms layout auto-generated code
─ Form1.resx # Resource file for GUI components
─ Microprogram.csproj # Project configuration for the application
─ Microprogram.sln # Visual Studio solution file

## 🚀 Getting Started

### Prerequisites

- Visual Studio 2019 or newer
- .NET Framework (4.x)

### Running the Simulator

1. Clone or download this repository.
2. Open the `Microprogram.sln` file in Visual Studio.
3. Build the solution (Ctrl + Shift + B).
4. Run the project (F5) to launch the simulator.

## 🧠 About the Architecture

This simulator implements a simplified **microprogrammed control unit** similar to the one described in Mano's model computer architecture. Key components include:

- **Control Memory:** Stores microinstructions that guide CPU behavior.
- **Control Logic:** Determines which microinstruction to execute next.
- **Registers:** Accumulator, Program Counter, Instruction Register, etc.
- **ALU:** Executes arithmetic and logic operations based on control signals.

The system provides a clear window into how a basic CPU processes instructions at the micro-operation level.

## 📄 License

This project is provided for educational purposes. Feel free to use, modify, and share it in accordance with your institution's guidelines.

## 👤 Author

Developed as part of a university project to demonstrate understanding of microprogrammed computer systems and control logic.

---
