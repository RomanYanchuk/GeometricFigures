import { Component } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { FiguresService } from '../services/figures.service';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-create',
  templateUrl: './create.component.html',
  styleUrls: ['./create.component.scss'],
})
export class CreateComponent {
  figureForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private figuresService: FiguresService,
    public router: Router,
    public snackbar: MatSnackBar
  ) {
    this.figureForm = this.fb.group({
      name: ['', Validators.required],
      area: ['', [Validators.required, this.positiveNumberValidator]],
      perimeter: ['', [Validators.required, this.positiveNumberValidator]],
    });
  }

  ngOnInit(): void {}

  createFigure(): void {
    if (this.figureForm.valid) {
      const figure = this.figureForm.value;
      this.figuresService.create(figure).subscribe(
        () => {
          // this.figureForm.reset();
          this.snackbar.open('Figure is successfully created!', 'OK');
          // this.router.navigate(['']);
        },
        (error) => {
          this.snackbar.open('Some error is occured. Please try again.', 'OK');
          console.error(error);
        }
      );
    }
  }

  positiveNumberValidator(control: AbstractControl): ValidationErrors | null {
    const value: string = control.value;

    if (value === null || value === undefined || value.trim() === '') {
      return { required: true };
    }

    if (!/^[0-9]*\.?[0-9]+$/.test(value) || Number(value) < 0) {
      return { invalidNumber: true };
    }

    return null;
  }
}
