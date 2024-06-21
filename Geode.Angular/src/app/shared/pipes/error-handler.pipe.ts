import { Pipe, PipeTransform } from '@angular/core';
import { ValidationErrors } from '@angular/forms';

@Pipe({
  name: 'errorHandler',
  standalone: true,
})
export class ErrorHandlerPipe implements PipeTransform {
  private errorResolver: any = {
    required: () => 'This field is required',
    minlength: () => 'Value is too short',
    email: () => 'Email should be in the valid format',
    passwordErrors: (errors: string[]) => errors.join(' '),
  };

  transform(errorKeys: ValidationErrors): string | null {
    let validationError = Object.keys(errorKeys)[0];
    let resolverAction = this.errorResolver[validationError];
    if (resolverAction) {
      return resolverAction(errorKeys[validationError]);
    }

    return null;
  }
}
