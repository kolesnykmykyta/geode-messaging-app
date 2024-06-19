import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function passwordValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    let passwordRules: { regex: RegExp; error: string }[] = [
      {
        regex: /[A-Z]+/, // Uppercase validation
        error: 'Passwords must have at least one uppercase.',
      },
      {
        regex: /[a-z]+/, // Lowercase validation
        error: 'Passwords must have at least one lowercase.',
      },
      {
        regex: /[0-9]+/, // Digit validation
        error: 'Passwords must have at least one digit.',
      },
      {
        regex: /[^a-zA-Z0-9]/, // Non alphanumeric validation
        error: 'Passwords must have at least one non alphanumeric character.',
      },
    ];

    let value = control.value;
    let errors = [];
    for (let rule of passwordRules) {
      if (!rule.regex.test(value)) {
        errors.push(rule.error);
      }
    }

    return errors.length === 0
      ? null
      : { invalidPassword: true, passwordValidationErrors: errors };
  };
}
