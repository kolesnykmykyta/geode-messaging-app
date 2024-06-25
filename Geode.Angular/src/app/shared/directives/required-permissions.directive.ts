import { Directive, Input, TemplateRef, ViewContainerRef } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Directive({
  selector: '[requiredPermissions]',
})
export class RequiredPermissionsDirective {
  @Input() set requiredPermissions(permissions: string[]) {
    for (let permission of permissions) {
      if (!this.authService.permissions.includes(permission)) {
        this.viewContainer.clear();
        return;
      }
    }

    this.viewContainer.createEmbeddedView(this.templateRef);
  }

  constructor(
    private templateRef: TemplateRef<unknown>,
    private viewContainer: ViewContainerRef,
    private authService: AuthService
  ) {}
}
