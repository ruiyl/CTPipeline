using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	public class TutorialManager : MonoBehaviour
	{
		[SerializeField] private GameManager gameManager;
		[SerializeField] private UIManager uiManager;
		[SerializeField] private BlockPlacerManager blockPlacerManager;
		[SerializeField] private GameObject bg;
		[SerializeField] private GameObject introPanel;
		[SerializeField] private GameObject stockAHighlight;
		[SerializeField] private GameObject stock1Highlight;
		[SerializeField] private GameObject outputHighlight;
		[SerializeField] private GameObject p1ControlHighlight;
		[SerializeField] private GameObject p2ControlHighlight;
		[SerializeField] private GameObject goalHighlight;
		[SerializeField] private TextFlowRunner controlTutorialTextFlow;
		[SerializeField] private GameObject controlTutorialBlock;
		[SerializeField] private TextFlowRunner planModeTutorialTextFlow;

		private TutorialStep step;
		private TutorialGoalKeeper goalKeeper;

		public enum TutorialStep
		{
			Undefined,
			Intro,
			Control,
			PlanMode,
			MergeBlock,
			LoopBlock,
			SwitchBlock,
			AdvanceBlock,
			End,
		}

		private void Start()
		{
			StartStep();
		}

		public void SetTutorialMode(TutorialGoalKeeper goalKeeper)
		{
			step = TutorialStep.Intro;
			this.goalKeeper = goalKeeper;
			this.goalKeeper.OnScoreAdded += HandleScoreAdded;
			this.goalKeeper.OnAllGoalsComplete += HandleAllGoalsComplete;
			controlTutorialBlock.SetActive(true);
			gameManager.PlanModeTrigger += HandlePlanModeTrigger;
			uiManager.SetPlanModeButton(false, false);
			uiManager.SetBlockIconLockState(true);
			blockPlacerManager.BlockPlaced += HandleBlockPlaced;
			blockPlacerManager.ConnectionCreated += HandleConnectionCreated;
		}

		public void OnTextFlowStep(TextFlowRunner textFlow, int index, int count)
		{
			switch (step)
			{
				case TutorialStep.Intro:
					if (index >= count)
					{
						EndStep();
					}
					break;
				case TutorialStep.Control:
					switch (index)
					{
						case 0:
							goalKeeper.GetNewGoals();
							goalHighlight.SetActive(true);
							break;
						case 2:
							goalHighlight.SetActive(false);
							stock1Highlight.SetActive(true);
							stockAHighlight.SetActive(true);
							outputHighlight.SetActive(true);
							break;
						case 3:
							stock1Highlight.SetActive(false);
							stockAHighlight.SetActive(false);
							outputHighlight.SetActive(false);
							p1ControlHighlight.SetActive(true);
							p2ControlHighlight.SetActive(true);
							break;
						case 4:
							p1ControlHighlight.SetActive(false);
							p2ControlHighlight.SetActive(false);
							controlTutorialBlock.SetActive(false);
							stock1Highlight.SetActive(true);
							stockAHighlight.SetActive(true);
							outputHighlight.SetActive(true);
							textFlow.SetNextButtonState(false);
							break;
						case 6:
							textFlow.SetNextButtonState(true);
							break;
					}
					if (index >= count)
					{
						EndStep();
					}
					break;
				case TutorialStep.PlanMode:
					switch (index)
					{
						case 1:
							goalKeeper.GetNewGoals();
							break;
						case 3:
							uiManager.SetPlanModeButton(true, true);
							textFlow.SetNextButtonState(false);
							break;
						case 4:
							uiManager.SetPlanModeButton(true, false);
							textFlow.SetNextButtonState(true);
							break;
						case 5:
							uiManager.SetBlockIconLockState(false, 0);
							textFlow.SetNextButtonState(false);
							break;
						case 6:
							uiManager.SetBlockIconLockState(false, 1);
							break;
						case 8:
							textFlow.SetNextButtonState(true);
							break;
						case 9:
							uiManager.SetPlanModeButton(true, true);
							textFlow.SetNextButtonState(false);
							break;
						case 11:
							textFlow.SetNextButtonState(true);
							break;
					}
					if (index >= count)
					{
						EndStep();
					}
					break;
			}
		}

		public void EndStep()
		{
			switch (step)
			{
				case TutorialStep.Intro:
					introPanel.SetActive(false);
					break;
				case TutorialStep.Control:
					stock1Highlight.SetActive(false);
					stockAHighlight.SetActive(false);
					outputHighlight.SetActive(false);
					controlTutorialTextFlow.gameObject.SetActive(false);
					break;
				case TutorialStep.PlanMode:
					planModeTutorialTextFlow.gameObject.SetActive(false);
					break;
			}
			step++;
			StartStep();
		}

		public void StartStep()
		{
			switch (step)
			{
				case TutorialStep.Intro:
					introPanel.SetActive(true);
					break;
				case TutorialStep.Control:
					controlTutorialTextFlow.gameObject.SetActive(true);
					goalKeeper.SetGeneratorStep(step);
					break;
				case TutorialStep.PlanMode:
					planModeTutorialTextFlow.gameObject.SetActive(true);
					break;
			}
		}

		private void HandleScoreAdded(int addedScore)
		{
			switch (step)
			{
				case TutorialStep.Control:
					if (addedScore < 0)
					{
						controlTutorialTextFlow.Next();
					}
					break;
			}
		}

		private void HandleAllGoalsComplete()
		{
			switch (step)
			{
				case TutorialStep.Control:
					controlTutorialTextFlow.SetStep(6, true);
					break;
				case TutorialStep.PlanMode:
					planModeTutorialTextFlow.SetStep(11, true);
					break;
			}
		}

		private void HandlePlanModeTrigger(bool isIn)
		{
			switch (step)
			{
				case TutorialStep.PlanMode:
					if (isIn)
					{
						planModeTutorialTextFlow.SetStep(4, true);
					}
					else
					{
						planModeTutorialTextFlow.SetStep(10, true);
					}
					break;
			}
		}

		private void HandleBlockPlaced(BlockMono block)
		{
			switch (step)
			{
				case TutorialStep.PlanMode:
					switch (block)
					{
						case StartBlockMono _:
							planModeTutorialTextFlow.SetStep(6, true);
							break;
						case UpgradeBlockMono _:
							planModeTutorialTextFlow.SetStep(7, true);
							break;
					}
					break;
			}
		}

		private void HandleConnectionCreated(PipelinePathMono pathMono)
		{
			switch (step)
			{
				case TutorialStep.PlanMode:
					planModeTutorialTextFlow.SetStep(8, true);
					break;
			}
		}
	}
}