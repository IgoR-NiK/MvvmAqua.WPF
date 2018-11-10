using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace MVVMAqua.Validation
{
	public class ValidatableProperty<T> : BindableObject, IValidity
	{
		private Action OnValueChanged { get; }

		public ValidatableProperty() : this(null, false) { }
		public ValidatableProperty(Action onChanged) : this(onChanged, false) { }
		public ValidatableProperty(bool isValidateWhenPropertyChange) : this(null, isValidateWhenPropertyChange) { }
		public ValidatableProperty(Action onValueChanged, bool isValidateWhenPropertyChange) 
		{
			OnValueChanged += onValueChanged;
			if (isValidateWhenPropertyChange)
			{
				OnValueChanged += () => Validate();
			}

			Errors.CollectionChanged += (sender, e) =>
			{
				if (e.Action == NotifyCollectionChangedAction.Add)
				{
					if (Errors.Count == 1)
					{
						OnPropertyChanged(nameof(IsValid));
						IsValidChanged?.Invoke();
					}
				}
				else if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Reset)
				{
					if (Errors.Count == 0)
					{
						OnPropertyChanged(nameof(IsValid));
						IsValidChanged?.Invoke();
					}
				}
			};
		}

		private List<ValidationRuleWrapper<T>> ValidationRules { get; } = new List<ValidationRuleWrapper<T>>();
		public ObservableCollection<ValidationResult> Errors { get; } = new ObservableCollection<ValidationResult>();

		public event Action IsValidChanged;

		private T _value;
		public T Value
		{
			get => _value;
			set => SetProperty(ref _value, value, OnValueChanged);
		}

		public bool IsValid => Errors.Count == 0;


		public void AddValidationRule(IValidationRule<T> rule)
		{
			ValidationRules.Add(new ValidationRuleWrapper<T>(rule.Check));
		}

		public void AddValidationRule(Func<T, ValidationResult> rule)
		{
			if (rule == null)
			{
				throw new ArgumentException("Не указано правило валидации.");
			}

			ValidationRules.Add(new ValidationRuleWrapper<T>(rule));
		}

		public bool Validate()
		{
			ValidationRules.ForEach(rule =>
			{
				var result = rule.ValidationRule(Value);
				result.Id = rule.Id;

				if (result.IsValid)
				{
					if (Errors.Contains(result))
					{
						Errors.Remove(result);
					}
				}
				else
				{
					if (!Errors.Contains(result))
					{
						Errors.Add(result);
					}
				}
			});

			return IsValid;			
		}
	}
}