   M S B A M L   `   `   `  ����   CyberAwerness     CyberChatBotWPF.MainWindow   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$(  "CyberBot - Cybersecurity Assistant��$	��450��$	��700�� � WindowStyle$
 None=� � AllowsTransparency$
 True��
������  � 
ResizeMode$ 	CanResize=� � 	Resources    ModernButton �� 
TargetType �� Property �� Value �� 
TargetType	 �� CornerRadius
 �� Padding �� Triggers P� Property P� Value  UserMessage  
BotMessage  InputBox ��Name��%t  &	        &	 �    &	 ;    &	 �    �� " ��.���  � �� �#0084FF��  � ��
 �������  � ��$ 0q���  � ��$
 15,8q���  � ��$ 14(���  � ��$ SemiBold"���  �@�Cursor$
 Hand����  � �� �� " ��.R��� #��z��$	 20��#
 z��._��� $��Center=�$��Center=� P�  �@J�IsMouseOver$
 True��. ���  � �� �#0066CCP�  � ��$
 True��. ���  � �� �#004499�� " ��.���  � �� �#0084FF��  �@��CornerRadius$ 18����  � ��$ 	60,5,10,5q���  �@��Padding$
 12,8q���  � ��$ Right=��� " ��.���  � �� �#F0F0F0��  �@��CornerRadius$ 18����  � ��$ 	10,5,60,5q���  �@��Padding$
 12,8q���  � ��$
 Left=��� " ��.���  � �� �#F8F8F8��  � ��
 �   ���  � ��$ 14(���  � ��$ 12q���  � ��$ 0q���  � ��$ Center=���  � ��$ 45����  �@��TextWrapping$
 Wrap=���  �@��AcceptsReturn$ False����  � �� �� " ��.R��� #��z��$	 25��#
 z��._��� $ PART_ContentHost��$��Center=�$��False��.���� 
�������$	 15��$��1q����#E0E0E0._��  � RowDefinitions �� $��60 ��� $��* ��� $��80 �.+��� $��0��
�������$	 	15,15,0,0�����#E0E0E0$��0,0,0,1q�._��  � ColumnDefinitions �� $
��Auto ��� $��* ��� $
��Auto �.+�A� $��0��$��40��$��40��$��	15,0,10,0q�����  �� 
StartPoint$	 0,0&� �� EndPoint$	 1,1&�.���  � Color$ #FF6B6B�� � Offset$ 0Q�� $ #4ECDC4��$ 1Q��� $��1��$��Center=�.+��� $��CyberChat 🛡️��$
��Bold"�$��16(��� $��Cybersecurity Assistant��$��12(����#666 J� Opacity$	 0.8Q��� -   $��2��$	��✕��$��30��$��30��
������ $��0q�$��16(�$��	10,0,15,0q��� -   $ ChatScrollViewer��$��1��$
��Auto=�$��Disabled=����#FAFAFA.���� -    ��Name$ 	ChatPanel��$��5q��� $��2��
�������$	 	0,0,15,15�����#E0E0E0$��0,1,0,0q�._�� $��15,10q� �� $
��Auto ��� $��* ��� $
��Auto �.+��� -   $��0��$
��📎��$��35��$��35��
������ $��0q�$��18(�$��0,0,10,0q��� -    ��Name$ UserInputBox��$��1��#��[ $	��100��$��	0,-45,0,0q����� *	UserInput+ �� UpdateSourceTrigger$ PropertyChanged=��� -   $��2��$	��➤��$��35��$��35��#��[  $��10,0,0,0q�� -    �Name$ QuickActionsPopup��$��1��$��Bottom=� � 	StaysOpen$ False�� � AllowsTransparency$
 True�� � PlacementTarget ��  �� ElementName$ 	ChatPanel��.���� 
�������$	 10��$��1q����#E0E0E0 J� Effect 3  -System.Windows.Media.Effects.DropShadowEffect     Color$ Black��    Opacity$	  0.1Q�!   
BlurRadius$! 10Q�"   ShadowDepth$" 2Q�._��� $��10q�.+��� -   $��🧠 Start Quiz��#��[  $��5q��� -	   $��🎮 Play Game��#��[  $��5q��� -
   $��📋 View Tasks��#��[  $��5q��� -   $��📊 Activity Log��#��[  $��5q��� -   $��	💡 Help��#��[  $��5q�