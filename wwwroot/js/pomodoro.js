let timerInterval;
let currentTimeInSeconds;
let isRunning = false;
let currentMode = 'Pomodoro';

document.addEventListener('DOMContentLoaded', function() {
    const timerElement = document.getElementById('timer');
    const musicPlayer = document.getElementById('musicPlayer');
    const alarmSound = document.getElementById('alarmSound');
    
    // Initialize timer from server data
    if (timerElement) {
        currentTimeInSeconds = parseInt(timerElement.getAttribute('data-timer')) || 1500; // Default 25 minutes
        updateTimerDisplay();
    }
    
    // Handle form submissions with AJAX to maintain timer state
    const pomodoroForm = document.getElementById('pomodoroForm');
    if (pomodoroForm) {
        const buttons = pomodoroForm.querySelectorAll('button[type="submit"]');
        buttons.forEach(button => {
            button.addEventListener('click', function(e) {
                e.preventDefault();
                const action = this.value;
                handleTimerAction(action);
            });
        });
    }
    
    // Auto-submit forms when dropdowns change
    const optionsForm = document.getElementById('optionsForm');
    if (optionsForm) {
        const selects = optionsForm.querySelectorAll('select');
        selects.forEach(select => {
            select.addEventListener('change', function() {
                // Stop current timer and music when changing options
                stopTimer();
                stopMusic();
                // Submit form to update background and music
                this.form.submit();
            });
        });
    }
});

function handleTimerAction(action) {
    stopTimer(); // Stop any existing timer
    
    switch(action) {
        case 'StartPomodoro':
            currentTimeInSeconds = 25 * 60; // 25 minutes
            currentMode = 'Pomodoro';
            startTimer();
            startMusic();
            break;
        case 'ShortBreak':
            currentTimeInSeconds = 5 * 60; // 5 minutes
            currentMode = 'Short Break';
            startTimer();
            stopMusic(); // No music during breaks
            break;
        case 'LongBreak':
            currentTimeInSeconds = 15 * 60; // 15 minutes
            currentMode = 'Long Break';
            startTimer();
            stopMusic(); // No music during breaks
            break;
        case 'Reset':
            currentTimeInSeconds = 25 * 60; // Reset to 25 minutes
            currentMode = 'Pomodoro';
            stopMusic();
            break;
    }
    
    updateTimerDisplay();
    updateModeDisplay();
}

function startTimer() {
    if (isRunning) return;
    
    isRunning = true;
    timerInterval = setInterval(function() {
        currentTimeInSeconds--;
        updateTimerDisplay();
        
        if (currentTimeInSeconds <= 0) {
            stopTimer();
            playAlarm();
            
            // Auto-switch to break or pomodoro
            if (currentMode === 'Pomodoro') {
                // After pomodoro, start short break
                setTimeout(() => handleTimerAction('ShortBreak'), 1000);
            } else {
                // After break, reset to pomodoro
                setTimeout(() => handleTimerAction('Reset'), 1000);
            }
        }
    }, 1000);
}

function stopTimer() {
    if (timerInterval) {
        clearInterval(timerInterval);
        timerInterval = null;
    }
    isRunning = false;
}

function updateTimerDisplay() {
    const timerElement = document.getElementById('timer');
    if (timerElement) {
        const minutes = Math.floor(currentTimeInSeconds / 60);
        const seconds = currentTimeInSeconds % 60;
        timerElement.textContent = `${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`;
    }
}

function updateModeDisplay() {
    const modeElements = document.querySelectorAll('h2');
    modeElements.forEach(element => {
        if (element.textContent.includes('Pomodoro') || 
            element.textContent.includes('Short Break') || 
            element.textContent.includes('Long Break')) {
            element.textContent = currentMode;
        }
    });
}

function startMusic() {
    const musicPlayer = document.getElementById('musicPlayer');
    if (musicPlayer && currentMode === 'Pomodoro') {
        musicPlayer.play().catch(error => {
            console.log('Music autoplay prevented by browser:', error);
            // You might want to show a message to user to manually start music
        });
    }
}

function stopMusic() {
    const musicPlayer = document.getElementById('musicPlayer');
    if (musicPlayer) {
        musicPlayer.pause();
        musicPlayer.currentTime = 0;
    }
}

function playAlarm() {
    const alarmSound = document.getElementById('alarmSound');
    if (alarmSound) {
        alarmSound.play().catch(error => {
            console.log('Alarm sound failed to play:', error);
        });
    }
}