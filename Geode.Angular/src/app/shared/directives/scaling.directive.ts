import {
  AfterViewInit,
  Directive,
  ElementRef,
  HostListener,
  Input,
} from '@angular/core';

@Directive({
  selector: '[scaling]',
})
export class ScalingDirective {
  @Input()
  scale: number = 1.2;
  constructor(private elementRef: ElementRef) {}

  @HostListener('mouseenter') scaleElement() {
    this.elementRef.nativeElement.style.transform = `scale(${this.scale})`;
  }

  @HostListener('mouseleave') shrinkElement() {
    this.elementRef.nativeElement.style.transform = 'scale(1)';
  }
}
