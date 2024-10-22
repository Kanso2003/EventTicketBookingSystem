import { Component, ElementRef, ViewChild } from '@angular/core';
import { CommonModule, NgIf } from '@angular/common';
import { RouterModule } from '@angular/router'; 
import { FormBuilder, FormGroup, Validators, AbstractControl, ValidationErrors, ReactiveFormsModule } from '@angular/forms';

// Custom validator function for email
export function emailValidator(control: AbstractControl): ValidationErrors | null {
  const email = control.value;
  
  // Regular expressions for validating the email domains
  const gmailPattern = /^[a-zA-Z0-9._%+-]+@gmail\.com$/; // Matches Gmail addresses
  const microsoftPattern = /^[a-zA-Z0-9._%+-]+@(outlook\.com|hotmail\.com|net\.usj\.edu\.lb)$/; // Matches Microsoft accounts

  // Check if the email matches either pattern
  if (email && !(gmailPattern.test(email) || microsoftPattern.test(email))) {
    return { invalidDomain: true }; // Invalid email domain
  }
  
  return null; // Valid email
}

// Custom validator for username
export function usernameValidator(control: AbstractControl): ValidationErrors | null {
  const username = control.value;
  // Check if username contains only letters, numbers, or underscores
  const validPattern = /^[a-zA-Z0-9_]+$/; 
  return username && !validPattern.test(username) ? { invalidUsername: true } : null;
}

@Component({
  selector: 'app-root',
  standalone: true,
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  imports: [CommonModule, NgIf, RouterModule, ReactiveFormsModule],  // Add ReactiveFormsModule here
})
export class AppComponent {
  title = 'EventBooking';

  @ViewChild('model') model: ElementRef | undefined;
  isLoginActive: boolean = true;

  phoneForm!: FormGroup;

  constructor(private formBuilder: FormBuilder) {
    this.createForm();  // Call createForm in the constructor
  }

  createForm() {
    this.phoneForm = this.formBuilder.group({
      email: ['', [Validators.required, emailValidator]], // Add email control
      countryCode: ['', [Validators.required]],  // Add country code field
      phone: ['', [Validators.required, Validators.pattern(/^\d+$/)]], // Only digits
      username: ['', [Validators.required,Validators.minLength(3),usernameValidator]], // Username cannot be empty
      password: ['', [
        Validators.required,
        Validators.minLength(8),
        Validators.pattern(/^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).*$/) // At least one uppercase, one lowercase, one number, and one special character
      ]], // Password validation
    });
  }

  openPopup() {
    if (this.model) {
      this.model.nativeElement.style.display = 'block';
    }
  }

  closePopup() {
    if (this.model) {
      this.model.nativeElement.style.display = 'none';
    }
  }

  switchToRegister() {
    this.isLoginActive = false;
  }

  switchToLogin() {
    this.isLoginActive = true;
  }

  register() {
    // Mark all fields as touched to show validation messages
    this.phoneForm.markAllAsTouched();
    
    // Check if the form is valid
    if (this.phoneForm.valid) {
      console.log('Registration successful', this.phoneForm.value);
      // Close the popup here if needed
      this.closePopup(); // Close the modal on successful registration
    } else {
      console.log('Registration failed due to errors');
      // No need to do anything else since the template handles displaying errors
    }
  }
  
}
