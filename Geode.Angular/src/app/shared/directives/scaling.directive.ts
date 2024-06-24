import { AfterViewInit, Directive, ElementRef, Input } from '@angular/core';

@Directive({
  selector: '[scaling]',
})
export class ScalingDirective implements AfterViewInit {
  @Input()
  scale: number = 1.2;
  constructor(private elementRef: ElementRef) {}

  ngAfterViewInit(): void {
    this.elementRef.nativeElement.style.transform = `scale(${this.scale})`;
  }
}
