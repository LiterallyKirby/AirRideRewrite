package main

import (
	"fmt"
	"os"
	"os/exec"
	"os/signal"
	"strings"
	"syscall"

	"golang.org/x/term"
)

func main() {
	// Save the current terminal state
	oldState, err := term.MakeRaw(int(os.Stdin.Fd()))
	if err != nil {
		panic(err)
	}
	defer term.Restore(int(os.Stdin.Fd()), oldState)

	// Handle Ctrl+C
	c := make(chan os.Signal, 1)
	signal.Notify(c, os.Interrupt, syscall.SIGTERM)
	go func() {
		<-c
		term.Restore(int(os.Stdin.Fd()), oldState)
		os.Exit(0)
	}()

	t := term.NewTerminal(os.Stdin, "> ")

	for {
		if !Update(t) {
			break
		}
	}
}

func Update(t *term.Terminal) bool {
	line, err := t.ReadLine()
	if err != nil {
		fmt.Fprintf(t, "\nError reading input: %v\n", err)
		return false
	}

	line = strings.TrimSpace(line)
	if line == "" {
		return true
	}

	// Handle built-in commands
	if strings.HasPrefix(line, "cd ") {
		dir := strings.TrimSpace(line[3:])
		if err := os.Chdir(dir); err != nil {
			fmt.Fprintf(t, "cd: %v\n", err)
		}
		return true
	}

	// Split command and arguments
	args := strings.Fields(line)
	if len(args) == 0 {
		return true
	}

	// Execute command
	cmd := exec.Command(args[0], args[1:]...)
	cmd.Stdin = os.Stdin
	cmd.Stdout = t
	cmd.Stderr = t

	if err := cmd.Run(); err != nil {
		fmt.Fprintf(t, "Error: %v\n", err)
	}

	return true
}
