import { Component } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { FiguresService } from '../services/figures.service';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Figure } from '../services/figure';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.scss'],
})
export class EditComponent {
  figureForm: FormGroup;
  id: number;
  isError: boolean;

  constructor(
    private activatedRoute: ActivatedRoute,
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

  ngOnInit(): void {
    this.id = this.activatedRoute.snapshot.params['id'];
    this.figuresService.getOne(this.id).subscribe(
      (figure: Figure) => {
        console.log(figure);
        this.figureForm.controls['name'].setValue(figure.name);
        this.figureForm.controls['area'].setValue(figure.area.toLocaleString());
        this.figureForm.controls['perimeter'].setValue(
          figure.perimeter.toLocaleString()
        );
      },
      () => {
        this.snackbar.open('Some error is occured. Please try again.', 'OK');
        this.isError = true;
      }
    );
  }

  updateFigure(): void {
    if (this.figureForm.valid) {
      const figure = this.figureForm.value;
      figure.id = this.id;
      this.figuresService.edit(figure).subscribe(
        () => {
          this.snackbar.open('Figure is successfully created!', 'OK');
          this.router.navigate(['']);
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
