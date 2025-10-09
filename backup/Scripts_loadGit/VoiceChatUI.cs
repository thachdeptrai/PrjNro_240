using System;
using UnityEngine;

public class VoiceChatUI : IActionListener
{
    private static VoiceChatUI instance;
    
    // UI Elements
    private Command cmdVoiceToggle;
    private Command cmdMuteToggle;
    private Command cmdVoiceSettings;
    
    // UI Positions
    private int voiceUIX;
    private int voiceUIY;
    private int buttonWidth = 60;
    private int buttonHeight = 25;
    private int buttonSpacing = 5;
    
    // Voice Indicator
    private bool showVoiceIndicator = false;
    private float indicatorAlpha = 0f;
    private long lastIndicatorTime = 0;
    
    public static VoiceChatUI gI()
    {
        if (instance == null)
        {
            instance = new VoiceChatUI();
        }
        return instance;
    }
    
    public void init()
    {
        // Position voice UI in top-center
        voiceUIX = (GameCanvas.w - 150) / 2; // Giảm width vì bớt 1 button
        voiceUIY = 10;
        
        // Initialize commands - bỏ cmdChannelSwitch
        cmdVoiceToggle = new Command("🎤 Voice OFF", this, 3001, null, voiceUIX, voiceUIY);
        cmdMuteToggle = new Command("Mute", this, 3003, null, voiceUIX + buttonWidth + buttonSpacing, voiceUIY);
        cmdVoiceSettings = new Command("Settings", this, 3004, null, voiceUIX, voiceUIY + buttonHeight + buttonSpacing);
    }
    
    public void update()
    {
        // Tạm dừng voice chat khi đang load map
        if (Char.isLoadingMap && VoiceChatManager.Instance != null)
        {
            VoiceChatManager.Instance.PauseVoiceChat();
            return;
        }
        
        // Resume voice chat khi không còn load map
        if (!Char.isLoadingMap && VoiceChatManager.Instance != null && VoiceChatManager.Instance.IsVoiceOn())
        {
            VoiceChatManager.Instance.ResumeVoiceChat();
        }
        
        // Update voice indicator animation
        if (showVoiceIndicator)
        {
            indicatorAlpha = (float)System.Math.Sin((mSystem.currentTimeMillis() - lastIndicatorTime) * 0.01) * 0.5f + 0.5f;
        }
        else
        {
            indicatorAlpha = 0f;
        }
        
        // Update button positions and captions
        UpdateButtonPositions();
        UpdateButtonCaptions();
        
        // Handle button clicks
        HandleButtonClicks();
    }
    
    private void UpdateButtonPositions()
    {
        // Recalculate center position
        voiceUIX = (GameCanvas.w - 150) / 2; // Giảm width
        
        // Update button positions - bỏ cmdChannelSwitch
        if (cmdVoiceToggle != null)
        {
            cmdVoiceToggle.x = voiceUIX;
            cmdVoiceToggle.y = voiceUIY;
        }
        if (cmdMuteToggle != null)
        {
            cmdMuteToggle.x = voiceUIX + buttonWidth + buttonSpacing;
            cmdMuteToggle.y = voiceUIY;
        }
        if (cmdVoiceSettings != null)
        {
            cmdVoiceSettings.x = voiceUIX;
            cmdVoiceSettings.y = voiceUIY + buttonHeight + buttonSpacing;
        }
    }
    
    private void HandleButtonClicks()
    {
        // Check if voice chat UI should be visible
        if (GameCanvas.panel.isShow || GameCanvas.currentDialog != null || ChatPopup.currChatPopup != null)
        {
            return;
        }
        
        // Handle voice toggle button click
        if (cmdVoiceToggle != null && cmdVoiceToggle.isPointerPressInside())
        {
            cmdVoiceToggle.performAction();
            return;
        } 
        
        // Handle mute toggle button click
        if (cmdMuteToggle != null && cmdMuteToggle.isPointerPressInside())
        {
            cmdMuteToggle.performAction();
            return;
        }
        
        // Handle voice settings button click
        if (cmdVoiceSettings != null && cmdVoiceSettings.isPointerPressInside())
        {
            cmdVoiceSettings.performAction();
            return;
        }
    }
    
    private void UpdateButtonCaptions()
    {
        if (VoiceChatManager.Instance == null) return;
        
        // Update voice toggle button
        if (VoiceChatManager.Instance.IsVoiceOn())
        {
            cmdVoiceToggle.caption = "🎤 Voice ON";
        }
        else
        {
            cmdVoiceToggle.caption = "🎤 Voice OFF";
        }
        
        // Update mute button
        if (VoiceChatManager.Instance.IsMuted())
        {
            cmdMuteToggle.caption = "Unmute";
        }
        else
        {
            cmdMuteToggle.caption = "Mute";
        }
    }
    
    public void paint(mGraphics g)
    {
        if (VoiceChatManager.Instance == null) return;
        
        // Don't show voice UI if in menu or dialog
        if (GameCanvas.panel.isShow || GameCanvas.currentDialog != null || ChatPopup.currChatPopup != null)
        {
            return;
        }
        
        // Paint voice buttons
        PaintVoiceButtons(g);
        
        // Paint voice indicator
        PaintVoiceIndicator(g);
        
        // Paint voice status
        PaintVoiceStatus(g);
    }
    
    private void PaintVoiceButtons(mGraphics g)
    {
        // Voice Toggle Button
        PaintButton(g, cmdVoiceToggle, VoiceChatManager.Instance.IsVoiceOn() ? 0x00FF00 : 0x808080);
        
        // Mute Button
        PaintButton(g, cmdMuteToggle, VoiceChatManager.Instance.IsMuted() ? 0xFF0000 : 0x808080);
        
        // Settings Button
        PaintButton(g, cmdVoiceSettings, 0x808080);
    }
    
    private void PaintButton(mGraphics g, Command cmd, int color)
    {
        // Button background
        g.setColor(color);
        g.fillRect(cmd.x, cmd.y, buttonWidth, buttonHeight);
        
        // Button border
        g.setColor(0x000000);
        g.drawRect(cmd.x, cmd.y, buttonWidth, buttonHeight);
        
        // Button text
        mFont font = VoiceChatManager.Instance.IsVoiceOn() ? mFont.tahoma_7b_white : mFont.tahoma_7_white;
        font.drawString(g, cmd.caption, cmd.x + buttonWidth / 2, cmd.y + buttonHeight / 2, mFont.CENTER);
    }
    
    private void PaintVoiceIndicator(mGraphics g)
    {
        if (showVoiceIndicator && indicatorAlpha > 0)
        {
            // Pulsing red circle - positioned above the buttons
            g.setColor(0xFF0000, (int)(indicatorAlpha * 255));
            g.fillRect(voiceUIX + 75, voiceUIY - 20, 10, 10); // Điều chỉnh vị trí
            
            // "Speaking..." text
            mFont.tahoma_7b_red.drawString(g, "Speaking...", voiceUIX + 75, voiceUIY - 25, mFont.CENTER);
        }
    }
    
    private void PaintVoiceStatus(mGraphics g)
    {
        if (VoiceChatManager.Instance.IsVoiceOn())
        {
            string status = VoiceChatManager.Instance.GetVoiceStatus();
            string channel = VoiceChatManager.Instance.GetChannelName();
            
            mFont.tahoma_7b_white.drawString(g, $"Voice: {status} | Channel: {channel}", 
                voiceUIX + 75, voiceUIY - 40, mFont.CENTER);
        }
    }
    
    private void ShowVoiceSettings()
    {
        // Create voice settings popup - bỏ team volume
        MyVector menuItems = new MyVector();
        menuItems.addElement(new Command("Master Volume: " + (int)(VoiceChatManager.Instance.masterVolume * 100) + "%", this, 4000, null));
        menuItems.addElement(new Command("All Volume: " + (int)(VoiceChatManager.Instance.allVolume * 100) + "%", this, 4002, null));
        menuItems.addElement(new Command("Close", this, 4003, null));
        
        GameCanvas.menu.startAt(menuItems, 0);
    }
    
    public void SetVoiceIndicator(bool show)
    {
        showVoiceIndicator = show;
        if (show)
        {
            lastIndicatorTime = mSystem.currentTimeMillis();
        }
    }
    
    public void handleMenuAction(int selectedIndex)
    {
        switch (selectedIndex)
        {
            case 4000: // Master Volume
                AdjustVolume("master");
                break;
            case 4002: // All Volume
                AdjustVolume("all");
                break;
            case 4003: // Close
                GameCanvas.menu.showMenu = false;
                break;
        }
    }
    
    private void AdjustVolume(string volumeType)
    {
        float currentVolume = 0f;
        switch (volumeType)
        {
            case "master":
                currentVolume = VoiceChatManager.Instance.masterVolume;
                break;
            case "all":
                currentVolume = VoiceChatManager.Instance.allVolume;
                break;
        }
        
        // Simple volume adjustment (increase by 10%)
        float newVolume = System.Math.Min(1f, currentVolume + 0.1f);
        VoiceChatManager.Instance.SetVolume(newVolume, volumeType);
        
        // Show updated settings
        ShowVoiceSettings();
    }
    
    public void perform(int idAction, object p)
    {
        if (idAction >= 4000)
        {
            handleMenuAction(idAction);
        }
        else
        {
            // Handle voice chat actions
            if (VoiceChatManager.Instance == null) return;
            
            switch (idAction)
            {
                case 3001: // Voice Toggle
                    VoiceChatManager.Instance.ToggleVoice();
                    if (VoiceChatManager.Instance.IsVoiceOn())
                    {
                        showVoiceIndicator = true;
                        lastIndicatorTime = mSystem.currentTimeMillis();
                    }
                    else
                    {
                        showVoiceIndicator = false;
                    }
                    break;
                    
                case 3003: // Mute Toggle
                    VoiceChatManager.Instance.ToggleMute();
                    break;
                    
                case 3004: // Voice Settings
                    ShowVoiceSettings();
                    break;
            }
        }
    }
}
