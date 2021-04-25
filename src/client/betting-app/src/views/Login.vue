<template>
  <div class="l-login container d-flex flex-column justify-content-center">
    <main>
      <section>
        <header>
          <h1 class="o-title">
            Log in
          </h1>
        </header>

        <b-form class="c-form__login" @submit.stop.prevent="onSubmit">
          <b-form-group id="example-input-group-1" label="Username:" label-for="example-input-1">
            <b-form-input
              id="example-input-1"
              v-model="$v.form.username.$model"
              name="example-input-1"
              aria-describedby="input-1-live-feedback"
            />

            <b-form-invalid-feedback
              id="input-1-live-feedback"
            >
              This is a required field.
            </b-form-invalid-feedback>
          </b-form-group>

          <b-form-group id="input-group-11" label="Password:" label-for="input-11">
            <b-form-input
              id="input-1"
              v-model="$v.form.password.$model"
              name="input-1"
              :type="'password'"      
              aria-describedby="live-feedback"
            />

            <b-form-invalid-feedback
              id="live-feedback"
            >
              This is a required field.
            </b-form-invalid-feedback>
          </b-form-group>

          <b-alert :show="loginFailed" variant="danger">
            Login failed: Invalid username and/or password
          </b-alert>

          <b-button type="submit" class="c-button btn-lg" variant="primary">
            Log in
          </b-button>
        </b-form>
      </section>
    </main>
    
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { required, minLength } from 'vuelidate/lib/validators';
Component.registerHooks(['validations']);

@Component
export default class Login extends Vue {
  loginFailed = false;
  form = {
    username: null,
    password: null
  }
  $v: any;
  $router: any;

  onFailure(test: any) {
    console.log(test);
    this.loginError();
  }

  validations() {
    return {
      form: {
        username: {
          required
        },
        password: {
          required,
          minLength: minLength(6),
        }
      }
    };
  }

  validateState(name: string|number) {
    const { $dirty, $error } = this.$v.form[name] || {};
    return $dirty ? !$error : null;
  }

  resetForm() {
    this.form = {
      username: null,
      password: null
    };

    this.$nextTick(() => {
      this.$v.$reset();
    });
  }

  onSubmit() {
    this.$v.form.$touch();
    if (this.$v.form.$anyError) {
      // return;
    }
    this.$emit('setOverlay', true);
    this.loginFailed = false;

  }

  goToPortal() {
    this.$emit('setOverlay', false);
    this.$router.push('/portal');
  }

  loginError() {
    this.loginFailed = true;
    this.$emit('setOverlay', false);
  }
}
</script>
